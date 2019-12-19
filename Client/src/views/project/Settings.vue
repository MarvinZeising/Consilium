<template>
  <v-container fluid>
    <v-layout wrap>

      <!--//* Main Heading -->
      <v-flex xs12 sm10 md8 lg6 xl4>
        <h2
          class="headline mb-3"
          v-t="'project.general'"
        />
        <v-card
          flat
          class="ma-2 mb-5"
        >
          <v-card-text>
            <p
              class="caption mb-0 grey--text"
              v-t="'core.name'"
            />
            <p class="subtitle-1">
              {{ getProject.name }}
            </p>

            <p
              class="caption mb-0 grey--text"
              v-t="'core.email'"
            />
            <p class="subtitle-1 mb-0">
              {{ getProject.email }}
            </p>
          </v-card-text>

          <v-card-actions>
            <v-spacer />
            <v-btn
              text
              :to="{ name: 'updateGeneral' }"
              v-t="'core.edit'"
            />
          </v-card-actions>
        </v-card>
      </v-flex>

      <Topics />

      <!--//* Danger Zone -->
      <v-flex xs12 sm10 md8 lg6 xl4>
        <h2
          class="headline mb-3 error--text"
          v-t="'core.dangerZone'"
        />
        <v-card
          flat
          dark
          color="red lighten-4"
          class="ma-2 mb-5"
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
import ProjectModule from '@/store/modules/projects'
import { Project, Topic } from '@/models/definitions'
import KnowledgeBaseModule from '../../store/modules/knowledgeBase'
import Topics from '../../components/Topics.vue'
import DeleteProjectDialog from '../../components/dialogs/DeleteProjectDialog.vue'

@Component({
  components: {
    DeleteProjectDialog,
    Topics,
  }
})
export default class Settings extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  public get getProject(): Project {
    const projectId = this.$route.params.projectId
    const projects = this.projectModule.myProjects.filter((project) => project.id === projectId)
    return projects.length > 0 ? projects[0] : new Project('Loading', 'Loading')
  }

}
</script>
