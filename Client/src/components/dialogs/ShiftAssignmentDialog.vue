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
      >
        <v-icon>thumbs_up_down</v-icon>
      </v-btn>
    </template>
    <v-card>
      <v-toolbar
        dark
        color="accent"
      >
        <v-toolbar-title>Handle Shift Applications</v-toolbar-title>
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
              <v-list-item-content>
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
                <v-list-item-content>
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
          type="submit"
          v-text="'Save draft'"
          :loading="loading"
          @click.stop="save"
        />
        <v-btn
          text
          type="submit"
          color="primary"
          v-text="'Release'"
          :loading="loading"
          @click.stop="save"
        />
      </v-card-actions>

    </v-card>
  </v-dialog>

</template>

<script lang="ts">
import moment from 'moment'
import { Vue, Component, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import UserModule from '../../store/users'
import PersonModule from '../../store/persons'
import ProjectModule from '../../store/projects'
import UpdateShiftDialog from './UpdateShiftDialog.vue'
import CreateApplicationDialog from './CreateApplicationDialog.vue'
import DeleteApplicationDialog from './DeleteApplicationDialog.vue'
import { Shift, Person } from '../../models'

@Component({
  components: {
    UpdateShiftDialog,
    CreateApplicationDialog,
    DeleteApplicationDialog,
  },
})
export default class ShiftAssignmentDialog extends Vue {
  private userModule = getModule(UserModule, this.$store)
  private personModule = getModule(PersonModule, this.$store)
  private projectModule = getModule(ProjectModule, this.$store)

  @Prop(Shift)
  private readonly shift?: Shift

  private dialog = false
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

}
</script>
