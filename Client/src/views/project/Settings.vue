<template>
  <v-container fluid>
    <v-layout wrap>

      <!--//* Main Heading -->
      <v-flex xs12>
        <h2 class="headline mb-3">General</h2>
      </v-flex>

      <!--//* Main Settings -->
      <v-flex
        xs12 sm8 md6 lg4
        class="mb-5 pa-2"
      >
        <v-card flat>
          <v-card-text>
            <p class="caption mb-0 grey--text">
              Name
            </p>
            <p class="subheading">
              {{ name }}
            </p>

            <p class="caption mb-0 grey--text">
              Email
            </p>
            <p class="subheading mb-0">
              {{ email }}
            </p>
          </v-card-text>

          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn
              flat
              :to="{ name: 'updateGeneral' }"
            >
              Edit
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-flex>

      <!--//* News Heading -->
      <v-flex xs12>
        <h2 class="headline mb-3">News</h2>
      </v-flex>

      <!--//* News Settings -->
      <v-flex
        xs12 sm8 md6 lg4
        class="mb-5 pa-2"
      >
        <v-card flat>
          <v-card-text>
            <p class="caption mb-0 grey--text">
              Project News
            </p>
            <p class="subheading mb-0">
              Lorem ipsum dolor sit amet consectetur adipisicing elit. Animi autem quas, veniam suscipit numquam sunt quo a eum eligendi rerum exercitationem doloremque velit. Veritatis odio, ratione commodi impedit earum cumque.
              <br>
              Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis, et, fuga eveniet nesciunt atque magnam, sunt non natus alias nobis culpa. Natus repudiandae, necessitatibus nam doloribus excepturi voluptatibus consectetur ad?
            </p>
          </v-card-text>
        </v-card>
      </v-flex>

      <!--//* Critical Heading -->
      <v-flex xs12>
        <h2 class="headline mb-3 error--text">Critical area</h2>
      </v-flex>

      <v-flex
        xs12 sm8 md6 lg4
        class="mb-5 pa-2"
      >
        <v-card
          flat
          dark
          color="red lighten-4"
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
import Vue from 'vue'
import DeleteProjectDialog from '@/components/dialogs/DeleteProjectDialog.vue'
import Component from 'vue-class-component'
import ProjectModule from '@/store/modules/projects'
import { getModule } from 'vuex-module-decorators'
import { Project } from '@/models/definitions'
import { Watch } from 'vue-property-decorator'

@Component({
  components: { DeleteProjectDialog }
})
export default class Settings extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  private name: string = ''
  private email: string = ''

  @Watch('$route')
  private async onRouteChanged(val: string, oldVal: string) {
    await this.loadProject()
  }

  private async created() {
    await this.loadProject()
  }

  private async loadProject() {
    const projectId = this.$route.params.projectId

    await this.projectModule.fetchProject(projectId)

    const project = this.projectModule.myProjects.filter((x: Project) => x.id === projectId)[0]
    if (project) {
      this.name = project.name
      this.email = project.email
    }
  }
}
</script>
