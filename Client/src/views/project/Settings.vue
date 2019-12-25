<template>
  <v-container fluid>
    <v-layout wrap>

      <SettingsGeneral />

      <Topics />

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
import Topics from '../../components/Topics.vue'
import DeleteProjectDialog from '../../components/dialogs/DeleteProjectDialog.vue'

@Component({
  components: {
    SettingsGeneral,
    DeleteProjectDialog,
    Topics,
  }
})
export default class Settings extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  public get getProject(): Project {
    const projectId = this.$route.params.projectId
    const projects = this.projectModule.getProjects.filter((project) => project.id === projectId)
    return projects.length > 0 ? projects[0] : new Project('Loading', 'Loading')
  }

}
</script>
