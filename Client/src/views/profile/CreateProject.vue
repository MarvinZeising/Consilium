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
            v-model="name"
            label="Project name"
            required
            counter
            maxlength="40"
            box
          />
          <v-text-field
            v-model="email"
            label="Project email"
            type="email"
            required
            box
          />

          <v-btn
            :disabled="name == '' || email == ''"
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
            v-text="name"
            class="title"
          />

          <p class="caption mb-0">Email</p>
          <p
            v-text="email"
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
import Component from 'vue-class-component';
import ProjectModule from '@/store/modules/projects';
import { getModule } from 'vuex-module-decorators';

@Component
export default class CreateProject extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  private e6: number = 1
  private name: string = ''
  private email: string = ''

  private async createProject() {
    const newProject = await this.projectModule.createProject({
      id: '',
      name: this.name,
      email: this.email
    })

    // TODO: change to project overview route (or first steps to configure the project)
    this.$router.push(`/project/${newProject.id}/calendar`)
  }

}
</script>
