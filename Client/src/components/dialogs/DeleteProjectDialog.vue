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
            v-model="projectName"
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
            :disabled="projectName == ''"
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
export default Vue.extend({
  data() {
    return {
      deleteProjectDialog: false,
      projectName: ''
    }
  },
  methods: {
    deleteProject() {
      const projectId = this.$route.params.projectId

      // TODO: check for correct project name

      axios.delete(`/projects/${projectId}`)
        .then((result) => {
          this.deleteProjectDialog = false
          this.$router.push('/home')
        })
        .catch((err) => {
          this.deleteProjectDialog = false
          console.error(err.toString())
        })
    }
  }
})
</script>
