<template>
  <v-container class="createProject">
    <h1 class="headline mb-4">Create a new Project</h1>

    <v-flex xs12 sm10 md8 lg6>
      <v-stepper v-model="e6" vertical>

        <v-stepper-step
          :complete="e6 > 1"
          step="1"
        >
          General
        </v-stepper-step>

        <v-stepper-content step="1">
          <v-text-field
            v-model="projectName"
            label="Project name"
            required
            counter
            maxlength="40"
            box
          />
          <v-text-field
            v-model="projectEmail"
            label="Project email"
            type="email"
            required
            box
          />

          <v-btn
            :disabled="projectName == '' || projectEmail == ''"
            @click="e6 = 2"
            color="primary"
          >
            Continue
          </v-btn>
          <v-btn flat>Cancel</v-btn>
        </v-stepper-content>

        <v-stepper-step
          :complete="e6 > 2"
          step="2"
        >
          Review
        </v-stepper-step>

        <v-stepper-content step="2">
          <p class="caption mb-0">Name</p>
          <p
            v-text="projectName"
            class="title"
          />

          <p class="caption mb-0">Email</p>
          <p
            v-text="projectEmail"
            class="title"
          />

          <v-btn
            @click="createProject"
            color="primary"
          >
            Create project
          </v-btn>
          <v-btn flat>Cancel</v-btn>
        </v-stepper-content>

      </v-stepper>

    </v-flex>
  </v-container>
</template>

<script lang="ts">
import Vue from 'vue'
import axios from '@/tools/axios'
export default Vue.extend({
  data() {
    return {
      e6: 1,
      projectName: '',
      projectEmail: ''
    }
  },
  methods: {
    createProject() {
      axios
        .post('/projects', {
          name: this.projectName,
          email: this.projectEmail
        })
        .then((result) => {
          const projectId = result.data.id
          this.$router.push(`/project/${projectId}/calendar`)
        })
    }
  }
})
</script>
