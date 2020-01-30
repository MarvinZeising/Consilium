<template>

  <v-row
    v-if="shift && assignments"
    class="my-4"
    no-gutters
  >
    <v-col
      sm="7"
      md="8"
      style="min-height:100px;"
    >

      <v-list v-if="shift.applications.length > 0">
        <v-list-item
          v-for="(application, applicationIndex) in shift.getApplications"
          :key="applicationIndex"
        >
          <v-list-item-content v-if="application.person">
            <v-list-item-title v-text="application.person.getFullName" />
            <v-list-item-subtitle
              v-if="application.person.congregation"
              v-text="application.person.congregation.name"
            />
            <!-- // TODO: use user-decided teams -->
            <div class="mt-1">
              <v-chip
                v-for="(team, teamIndex) in projectModule.getActiveProject.getTeams"
                :key="teamIndex"
                class="mt-1 mr-1"
                v-text="team.name"
                small
                :color="getChipColor(application.personId, team.id)"
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

    <v-divider
      v-if="!$vuetify.breakpoint.smAndDown"
      vertical
    />

    <v-col>

      <v-card-text
        v-if="$vuetify.breakpoint.smAndDown"
        class="pb-0"
      >
        <v-divider class="mb-4" />
        <span
          v-t="'shift.status.assignments'"
          class="title"
        />
      </v-card-text>

      <v-list
        v-if="getHasAttendee"
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
              v-t="'shift.attendee.noCaptain'"
              class="ml-1"
              color="error"
            />
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
                <v-list-item-title
                  v-t="'shift.attendee.demote'"
                  class="ml-2"
                />
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
                <v-list-item-title
                  v-t="'shift.attendee.promote'"
                  class="ml-2"
                />
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

</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import ProjectModule from '../../store/projects'
import { Shift } from '../../models'

@Component
export default class ShiftAssignmentDialogPlanning extends Vue {
  private projectModule = getModule(ProjectModule, this.$store)

  @Prop(Shift)
  private readonly shift?: Shift

  @Prop(Object)
  private readonly assignments?: {
    [personId: string]: {
      teamId: string,
      isCaptain: boolean
    }
  }

  private get getHasAttendee() {
    if (this.assignments) {
      for (const personId of Object.keys(this.assignments)) {
        if (this.assignments[personId] !== undefined) {
          return true
        }
      }
      return false
    }
  }

  private getAttendees(teamId: string) {
    if (this.assignments) {
      const assignments = this.assignments

      return Object.keys(this.assignments)
        .filter((x) => assignments[x].teamId === teamId)
        .map((x) => this.shift?.applications.find((y) => y.personId === x)?.person)
        .sort((a, b) => {
          if (a && b) {
            if (assignments[a.id].isCaptain && !assignments[b.id].isCaptain) {
              return -1
            } else if (!assignments[a.id].isCaptain && assignments[b.id].isCaptain) {
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
  }

  private getChipColor(personId: string, teamId: string) {
    if (this.assignments) {
      const isInTeam = this.assignments[personId]?.teamId === teamId
      if (isInTeam) {
        if (this.shift?.isPlanned) {
          return 'navbar'
        } else if (this.shift?.isScheduled) {
          return 'green'
        } else if (this.shift?.isSuspended) {
          return 'red'
        }
      }
      return ''
    }
  }

  private toggleTeam(personId: string, teamId: string) {
    if (this.assignments) {
      if (this.assignments[personId]?.teamId === teamId) {
        Vue.set(this.assignments, personId, undefined)
      } else {
        Vue.set(this.assignments, personId, {
          teamId,
          isCaptain: false,
        })
      }
    }
  }

  private demote(personId: string) {
    if (this.assignments) {
      const item = this.assignments[personId]
      item.isCaptain = false
      Vue.set(this.assignments, personId, item)
    }
  }

  private promote(personId: string) {
    if (this.assignments) {
      const item = this.assignments[personId]
      item.isCaptain = true
      Vue.set(this.assignments, personId, item)
    }
  }

  private openPerson() {}

}
</script>
