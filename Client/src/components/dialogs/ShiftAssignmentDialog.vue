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
        <v-col cols="8">

          <v-list>
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
                    :color="assignments[application.personId] === team.id ? 'accent' : ''"
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

        </v-col>
        <v-divider vertical />
        <v-col>

          <v-list
            class="mx-2"
            dense
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
              <v-list-item
                v-for="(attendee, attendeeIndex) in getAttendees(team.id)"
                :key="attendeeIndex"
              >
                <v-list-item-content v-if="attendee">
                  <v-list-item-title v-text="attendee.getFullName" />
                </v-list-item-content>
              </v-list-item>
            </div>
          </v-list>

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

  private savedAssignments: any = {}
  private assignments: any = {}

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
      if (this.assignments[personId] !== this.savedAssignments[personId]) {
        return false
      }
    }

    return true
  }

  private opened() {
    if (this.shift?.attendees) {
      for (const attendee of this.shift.attendees) {
        Vue.set(this.assignments, attendee.personId, attendee.teamId)
      }
      Object.assign(this.savedAssignments, this.assignments)
    }
  }

  private toggleTeam(personId: string, teamId: string) {
    if (this.assignments[personId] === teamId) {
      Vue.set(this.assignments, personId, undefined)
    } else {
      Vue.set(this.assignments, personId, teamId)
    }
  }

  private getAttendees(teamId: string) {
    return Object.keys(this.assignments)
      .filter((x) => this.assignments[x] === teamId)
      .map((x) => this.shift?.applications.find((y) => y.personId === x)?.person)
  }

  private openPerson() {}

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
            teamId: this.assignments[personId],
            isCaptain: false,
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

  @Emit('saved') private emitSaved() { /* nothing to do */ }

}
</script>
