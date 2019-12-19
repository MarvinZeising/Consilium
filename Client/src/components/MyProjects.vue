<template>
  <div class="myProjects">

    <!--//* My Projects -->
    <v-flex xs12>
      <h2 class="headline mb-3">Your Projects</h2>
    </v-flex>

    <!--//* No Projects existing -->
    <v-flex v-if="projects == []">
      <i>You don't have access to any Projects, yet.</i>
    </v-flex>

    <!--//* Data -->
    <v-flex
      v-for="(project, i) in projects"
      :key="i"
      xs12 sm10 md8 lg6
      class="mb-5 pa-2"
    >
      <v-card>
        <v-card-title primary-title>
          <h3 class="title mb-0 ml-2">
            {{ project.name }}
          </h3>
        </v-card-title>

        <v-card-actions>
          <v-btn
            flat
            color="primary"
          >
            Show permissions
          </v-btn>
          <v-spacer />
          <v-btn
            flat
            color="error"
          >
            Leave Project
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-flex>

  </div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import ProjectModule from '@/store/modules/projects'
import { Project } from '@/models/definitions'

@Component
export default class MyProjects extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  private projects: Project[] = []

  private created() {
    this.projects = this.projectModule.myProjects
  }

}
</script>
