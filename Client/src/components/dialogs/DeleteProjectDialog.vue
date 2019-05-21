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
        <v-form v-model="valid">
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
              :rules="enteredNameRules"
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
              :disabled="!valid"
              type="submit"
              flat
              color="error"
              @click="deleteProject"
            >
              Delete
            </v-btn>
          </v-card-actions>
        </v-form>
      </v-card>
    </v-dialog>
  </v-layout>
</template>

<script lang="ts">
import Vue from 'vue'
import axios from 'axios'
import Component from 'vue-class-component'
import ProjectModule from '@/store/modules/projects'
import { getModule } from 'vuex-module-decorators'
import { Project } from '../../models/definitions'
import { Watch } from 'vue-property-decorator';
import { VForm } from 'vuetify/lib'

@Component({})
export default class DeleteProjectDialog extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  private valid: any = false
  private deleteProjectDialog: boolean = false
  private projectName: string = ''
  private enteredName: string = ''
  private get enteredNameRules() {
    return [
      (v: string) => !!v || 'Project name is required',
      (v: string) => v === this.projectName || 'Project name must be this Project\'s name',
    ]
  }

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
      this.projectName = project.name
    }
  }

  private async deleteProject() {
    const projectId = this.$route.params.projectId

    await this.projectModule.deleteProject(projectId)

    this.deleteProjectDialog = false
    this.$router.push({ name: 'home' })
  }

}
</script>
