<template>
  <v-layout>
    <v-dialog v-model="deleteProjectDialog" persistent max-width="600px">
      <template v-slot:activator="{ on }">
        <v-btn
          v-on="on"
          color="error"
        >
          Delete this Project
        </v-btn>
      </template>
      <v-card>
        <v-card-title>
          <span class="headline">Delete Project</span>
        </v-card-title>
        <v-card-text>
          <p class="subheading">
            This will delete all categories, teams and shifts. It will purge the knowledge base, revoke any access to the Project and delete it.
          </p>
          <p class="subheading">
            Enter the Project name to continue
          </p>
          <v-text-field
            v-model="enteredName"
            label="Project name"
            box
            required
          ></v-text-field>
          <p class="subheading text-uppercase error--text">
            !!! This cannot be undone - everything will be gone !!!
          </p>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            flat
            color="black"
            @click="deleteProjectDialog = false"
          >
            Cancel
          </v-btn>
          <v-btn
            :disabled="enteredName == ''"
            flat
            color="error"
            @click="deleteProject"
          >
            Delete
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-layout>
</template>

<script lang="ts">
import Vue from 'vue'
import axios from '@/tools/axios'
import Component from 'vue-class-component'
import ProjectModule from '@/store/modules/projects'
import { getModule } from 'vuex-module-decorators'
import { Project } from '../../models/definitions'

@Component({})
export default class DeleteProjectDialog extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  private deleteProjectDialog: boolean = false
  private projectName: string = ''
  private enteredName: string = ''

  private created() {
    const projectId = this.$route.params.projectId
    const project = this.projectModule.myProjects.filter((x: Project) => x.id === projectId)[0]
    this.projectName = project.name
  }

  private async deleteProject() {
    const projectId = this.$route.params.projectId

    if (this.projectName === this.enteredName) {
      await this.projectModule.deleteProject(projectId)

      this.deleteProjectDialog = false
      this.$router.push({ name: 'home' })
    } else {
      alert('Wrong project name!')
    }
  }

}
</script>
