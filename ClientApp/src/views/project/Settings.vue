<template>
  <v-container fluid>
    <v-layout wrap>
      <SettingsGeneral />

      <SettingsRoles />

      <SettingsCategories />

      <SettingsTeams />

      <SettingsTopics />

      <v-flex v-if="canView" xs12 sm10 md8 lg6 xl4>
        <v-card outlined class="ma-2 mb-5" style="border-color:#f00;">
          <v-card-text class="text--primary">
            <h2 class="headline mb-5" v-t="'core.dangerZone'" />
            <DeleteProjectDialog />
          </v-card-text>
        </v-card>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import ProjectModule from '../../store/projects'
import PersonModule from '../../store/persons'
import { Project, Topic } from '../../models'
import KnowledgeBaseModule from '../../store/knowledgeBase'
import SettingsGeneral from '../../components/SettingsGeneral.vue'
import SettingsTopics from '../../components/SettingsTopics.vue'
import SettingsRoles from '../../components/SettingsRoles.vue'
import SettingsCategories from '../../components/SettingsCategories.vue'
import SettingsTeams from '../../components/SettingsTeams.vue'
import DeleteProjectDialog from '../../components/dialogs/DeleteProjectDialog.vue'

@Component({
  components: {
    SettingsGeneral,
    DeleteProjectDialog,
    SettingsTopics,
    SettingsRoles,
    SettingsCategories,
    SettingsTeams,
  },
})
export default class Settings extends Vue {
  private projectModule = getModule(ProjectModule, this.$store)
  private personModule = getModule(PersonModule, this.$store)

  private get canView() {
    return this.personModule.getActiveRole?.settingsWrite === true
  }
}
</script>
