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
      xs12 sm8 md6 lg4
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
          <v-spacer></v-spacer>
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
import Vue from 'vue'
import Component from 'vue-class-component'
import ProjectModule from '@/store/modules/projects'
import { getModule } from 'vuex-module-decorators'
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
