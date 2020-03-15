<template>
  <v-flex v-if="canView" xs12 sm10 md8 lg6 xl4>
    <v-card flat class="ma-2 mb-5" :loading="loading">
      <v-card-text class="text--primary">
        <h2 class="headline">{{ $tc('shift.team.teams', 2) }}</h2>
        <span class="grey--text" v-t="'shift.team.description'" />
      </v-card-text>

      <v-list v-if="projectModule.getActiveProject">
        <!--// TODO: blah -->
        <v-list-item
          v-if="projectModule.getActiveProject.getTeams.length === 0"
        >{{ $tc('shift.team.teams', 0) }}</v-list-item>
        <v-list-item v-for="(team, index) in projectModule.getActiveProject.getTeams" :key="index">
          <v-list-item-content>
            <v-list-item-title v-text="team.name" />
          </v-list-item-content>
          <v-list-item-action>
            <UpdateTeamDialog :team="team" />
          </v-list-item-action>
        </v-list-item>
      </v-list>
      <v-card-actions v-if="!loading">
        <v-spacer />
        <CreateTeamDialog />
      </v-card-actions>
    </v-card>
  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import { Role } from '../models'
import CreateTeamDialog from '../components/dialogs/CreateTeamDialog.vue'
import UpdateTeamDialog from '../components/dialogs/UpdateTeamDialog.vue'
import PersonModule from '../store/persons'
import ProjectModule from '../store/projects'
import CategoryModule from '../store/categories'
import TeamModule from '../store/teams'

@Component({
  components: {
    CreateTeamDialog,
    UpdateTeamDialog,
  },
})
export default class SettingsTeams extends Vue {
  private personModule = getModule(PersonModule, this.$store)
  private projectModule = getModule(ProjectModule, this.$store)
  private teamModule = getModule(TeamModule, this.$store)

  private loading = true

  private get canView() {
    return this.personModule.getActiveRole?.calendarWrite === true
  }

  @Watch('personModule.getActivePerson')
  private async onPersonChanged(val: string, oldVal: string) {
    if (this.canView) {
      await this.init()
    }
  }

  @Watch('$route')
  private async onRouteChanged(val: string, oldVal: string) {
    if (this.canView) {
      await this.init()
    }
  }

  private async created() {
    if (this.canView) {
      await this.init()
    }
  }

  private async init() {
    this.loading = true

    await this.teamModule.loadTeams()

    this.loading = false
  }
}
</script>
