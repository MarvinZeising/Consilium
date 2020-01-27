<template>

  <v-dialog
    v-model="dialog"
    max-width="1000px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        icon
        title="TODO: add title"
        @click="opened"
      >
        <v-icon>thumbs_up_down</v-icon>
      </v-btn>
    </template>
    <v-card>
      <v-toolbar
        flat
        color="accent"
      >
        <v-toolbar-title>Handle Shift Applications</v-toolbar-title>
        <v-spacer />
        <v-toolbar-title>DRAFT</v-toolbar-title>
      </v-toolbar>

      <v-row
        class="my-4"
        no-gutters
      >
        <v-col
          cols="8"
          style="min-height:100px;"
        >

          <v-list v-if="shift.applications.length > 0">
            <v-list-item
              v-for="(application, applicationIndex) in shift.applications"
              :key="applicationIndex"
            >
              <v-list-item-content v-if="application.person">
                <v-list-item-title v-text="application.person.getFullName" />
                <v-list-item-subtitle v-text="application.person.congregation.name" />
                <!-- // TODO: use user-decided teams -->
                <div class="mt-1">
                  <v-chip
                    v-for="(team, teamIndex) in projectModule.getActiveProject.getTeams"
                    :key="teamIndex"
                    class="mt-1 mr-1"
                    v-text="team.name"
                    small
                    :color="personIsInTeam(application.personId, team.id) ? 'accent' : ''"
                    @click="toggleTeam(application.personId, team.id)"
                  />
                </div>
              </v-list-item-content>
              <v-list-item-action>
                <v-btn
                  icon
                  @click="openPerson()"
                >
                  <v-icon>info</v-icon>
                </v-btn>
              </v-list-item-action>
            </v-list-item>
          </v-list>
          <div
            v-else
            class="d-flex align-center justify-center"
            style="height:100%;"
          >
            <div v-text="$tc('shift.application.applicants', 0)" />
          </div>

        </v-col>
        <v-divider vertical />
        <v-col>

          <v-list
            v-if="getHasAttendee"
            class="mx-2"
            dense
            rounded
          >
            <div
              v-for="(team, teamIndex) in projectModule.getActiveProject.getTeams"
              :key="teamIndex"
            >
              <v-subheader v-if="getAttendees(team.id).length > 0">
                {{ team.name }}
                <v-spacer />
                <v-chip
                  x-small
                  class="ml-1"
                  color="error"
                >
                  No captain
                </v-chip>
                <v-chip
                  x-small
                  class="ml-1"
                  color="error"
                >
                  {{ getAttendees(team.id).length }} / 5
                </v-chip>
              </v-subheader>

              <v-menu
                v-for="(attendee, attendeeIndex) in getAttendees(team.id)"
                :key="attendeeIndex"
                offset-x
              >
                <template v-slot:activator="{ on: menu }">
                  <v-list-item v-on="{ ...menu }">
                    <v-list-item-content v-if="attendee">
                      <v-list-item-title>
                        <u v-if="assignments[attendee.id].isCaptain">{{ attendee.getFullName }}</u>
                        <span v-else>{{ attendee.getFullName }}</span>
                      </v-list-item-title>
                    </v-list-item-content>
                    <v-list-item-action>
                      <v-icon small>more_horiz</v-icon>
                    </v-list-item-action>
                  </v-list-item>
                </template>
                <v-list
                  v-if="attendee"
                  dense
                >
                  <v-list-item
                    v-if="assignments[attendee.id].isCaptain"
                    @click="demote(attendee.id)"
                  >
                    <v-list-item-avatar
                      class="ma-0"
                      min-width="20px"
                      width="20px"
                      height="20px"
                      left
                    >
                      <v-icon small>outlined_flag</v-icon>
                    </v-list-item-avatar>
                    <v-list-item-title class="ml-2">Demote from captain</v-list-item-title>
                  </v-list-item>
                  <v-list-item
                    v-else
                    @click="promote(attendee.id)"
                  >
                    <v-list-item-avatar
                      class="ma-0"
                      min-width="20px"
                      width="20px"
                      height="20px"
                      left
                    >
                      <v-icon small>flag</v-icon>
                    </v-list-item-avatar>
                    <v-list-item-title class="ml-2">Promote to captain</v-list-item-title>
                  </v-list-item>
                </v-list>
              </v-menu>

            </div>
          </v-list>
          <div
            v-else
            class="d-flex align-center justify-center"
            style="height:100%;"
          >
            <div v-text="$tc('shift.attendee.attendees', 0)" />
          </div>

        </v-col>
      </v-row>

      <v-divider />

      <v-card-actions>
        <v-btn
          text
          v-t="'core.cancel'"
          @click.stop="dialog = false"
        />
        <v-spacer />
        <v-btn
          text
          type=""
          v-text="'Reset draft'"
          :disabled="saving || releasing || isSaved"
          @click.stop="reset"
        />
        <v-btn
          text
          v-text="'Save draft'"
          :loading="saving"
          :disabled="releasing || isSaved"
          @click.stop="save"
        />
        <v-btn
          text
          color="primary"
          v-text="'Release'"
          :loading="releasing"
          :disabled="saving"
          @click.stop="release"
        />
      </v-card-actions>

    </v-card>
  </v-dialog>

</template>

<script lang="ts">
import moment from 'moment'
import { Vue, Component, Prop, Emit } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import UserModule from '../../store/users'
import PersonModule from '../../store/persons'
import ProjectModule from '../../store/projects'
import ShiftModule from '../../store/shifts'
import { Shift, Person } from '../../models'

@Component
export default class ShiftAssignmentDialog extends Vue {
  private userModule = getModule(UserModule, this.$store)
  private personModule = getModule(PersonModule, this.$store)
  private projectModule = getModule(ProjectModule, this.$store)
  private shiftModule = getModule(ShiftModule, this.$store)

  @Prop(Shift)
  private readonly shift?: Shift

  private dialog = false
  private saving = false
  private releasing = false

  private savedAssignments: {
    [personId: string]: {
      teamId: string,
      isCaptain: boolean
    }
  } = {}
  private assignments: {
    [personId: string]: {
      teamId: string,
      isCaptain: boolean
    }
  } = {}

  private get canEdit() {
    const role = this.personModule.getActiveRole
    if (role && role.calendarWrite) {
      const eligibility = role.eligibilities.find((x) => x.categoryId === this.shift?.categoryId)
      if (eligibility) {
        return eligibility.shiftsWrite
      }
    }
    return false
  }

  private get isSaved() {
    if (this.assignments.length !== this.savedAssignments.length) {
      return false
    }

    for (const personId of Object.keys(this.assignments)) {
      if (this.assignments[personId]?.teamId !== this.savedAssignments[personId]?.teamId
       || this.assignments[personId]?.isCaptain !== this.savedAssignments[personId]?.isCaptain) {
        return false
      }
    }

    return true
  }

  private get getHasAttendee() {
    for (const personId of Object.keys(this.assignments)) {
      if (this.assignments[personId] !== undefined) {
        return true
      }
    }
    return false
  }

  private opened() {
    Vue.set(this, 'assignments', {})
    Vue.set(this, 'savedAssignments', {})

    if (this.shift?.attendees) {
      for (const attendee of this.shift.attendees) {
        Vue.set(this.assignments, attendee.personId, {
          teamId: attendee.teamId,
          isCaptain: attendee.isCaptain,
        })
      }
      Object.assign(this.savedAssignments, this.assignments)
    }
  }

  private getAttendees(teamId: string) {
    return Object.keys(this.assignments)
      .filter((x) => this.assignments[x] && this.assignments[x].teamId === teamId)
      .map((x) => this.shift?.applications.find((y) => y.personId === x)?.person)
      .sort((a, b) => {
        if (a && b) {
          if (this.assignments[a.id].isCaptain && !this.assignments[b.id].isCaptain) {
            return -1
          } else if (!this.assignments[a.id].isCaptain && this.assignments[b.id].isCaptain) {
            return 1
          } else if (a.lastname < b.lastname) {
            return -1
          } else if (a.lastname > b.lastname) {
            return 1
          } else if (a.firstname < b.firstname) {
            return -1
          } else if (a.firstname > b.firstname) {
            return 1
          } else {
            return 0
          }
        } else {
          return 0
        }
      })
  }

  private personIsInTeam(personId: string, teamId: string) {
    return this.assignments[personId]?.teamId === teamId
  }

  private async reset() {
    const copy = {}
    Object.assign(copy, this.savedAssignments)
    Vue.set(this, 'assignments', copy)
  }

  private async save() {
    if (this.shift) {
      this.saving = true

      const assignments = Object.keys(this.assignments)
        .filter((personId) => this.assignments[personId] !== undefined)
        .map((personId) => {
          return {
            personId,
            ...this.assignments[personId],
          }
        })

      await this.shiftModule.makeAssignment({
        shiftId: this.shift?.id,
        assignments,
      })

      const copy = {}
      Object.assign(copy, this.assignments)
      Vue.set(this, 'savedAssignments', copy)

      this.saving = false
      this.emitSaved()
      this.dialog = true
    }
  }

  private async release() {
    if (this.shift) {
      this.releasing = true

      if (!this.isSaved) {
        await this.save()
      }

      this.releasing = false
    }
  }

  private toggleTeam(personId: string, teamId: string) {
    if (this.assignments[personId]?.teamId === teamId) {
      Vue.set(this.assignments, personId, undefined)
    } else {
      Vue.set(this.assignments, personId, {
        teamId,
        isCaptain: false,
      })
    }
  }

  private demote(personId: string) {
    const item = this.assignments[personId]
    item.isCaptain = false
    Vue.set(this.assignments, personId, item)
  }

  private promote(personId: string) {
    const item = this.assignments[personId]
    item.isCaptain = true
    Vue.set(this.assignments, personId, item)
  }

  private openPerson() {}

  @Emit('saved') private emitSaved() { /* nothing to do */ }

}
</script>
