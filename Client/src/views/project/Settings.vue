<template>
  <v-container fluid>
    <v-layout wrap>

      <SettingsGeneral />

      <SettingsTopics />

      <SettingsRoles />

      <!--//* Danger Zone -->
      <v-flex xs12 sm10 md8 lg6 xl4>
        <h2
          class="headline mb-3"
          v-t="'core.dangerZone'"
        />
        <v-card
          outlined
          class="ma-2 mb-5"
          style="border-color:#f00;"
        >
          <v-card-text>
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
import { Project, Topic } from '../../models/definitions'
import KnowledgeBaseModule from '../../store/knowledgeBase'
import SettingsGeneral from '../../components/SettingsGeneral.vue'
import SettingsTopics from '../../components/SettingsTopics.vue'
import SettingsRoles from '../../components/SettingsRoles.vue'
import DeleteProjectDialog from '../../components/dialogs/DeleteProjectDialog.vue'

@Component({
  components: {
    SettingsGeneral,
    DeleteProjectDialog,
    SettingsTopics,
    SettingsRoles,
  }
})
export default class Settings extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)
}
</script>
