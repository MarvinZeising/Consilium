<template>
  <v-container class="createProject">
    <h1 class="headline mb-4">Create a new Project</h1>

    <v-form
      ref="form"
      v-model="valid"
    >

      <v-flex xs12 sm10 md8 lg6>
        <v-stepper v-model="activeStep" vertical>

          <v-stepper-step
            :complete="activeStep > 1"
            step="1"
          >
            General
          </v-stepper-step>

          <v-stepper-content step="1">
            <p class="mt-4 grey--text text--darken-1">
              The name of this Project. The Name has to be unique accross all Projects.
            </p>
            <v-text-field
              v-model="name"
              label="Project name"
              :rules="nameRules"
              counter="40"
              box
              required
            />

            <p class="mt-4 grey--text text--darken-1">
              We'll use this Email address as reply-to in all Emails that we send on behalf of this Project.
              <br>
              You can create an Email account specifically for the project, or just use your own Email address.
            </p>
            <v-text-field
              v-model="email"
              label="Project email"
              type="email"
              :rules="emailRules"
              box
              required
            />

            <v-btn
              :disabled="!valid"
              @click="validateStep"
              color="primary"
            >
              Continue
            </v-btn>
            <v-btn
              flat
              @click="goBack"
            >
              Cancel
            </v-btn>
          </v-stepper-content>

          <v-stepper-step
            :complete="activeStep > 2"
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
            <v-btn
              flat
              @click="activeStep = 1"
            >
              Back
            </v-btn>
          </v-stepper-content>

        </v-stepper>
      </v-flex>

    </v-form>

  </v-container>
</template>

<script lang="ts">
import Vue from 'vue'
import axios from '@/tools/axios'
import Component from 'vue-class-component'
import ProjectModule from '@/store/modules/projects'
import { getModule } from 'vuex-module-decorators'

@Component
export default class CreateProject extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  private activeStep: number = 1
  private valid: boolean = false

  private name: string = ''
  private nameRules: any[] = [
    (v: string) => !!v || 'Name is required',
    (v: string) => v.length <= 40 || 'Name must be less than 40 characters',
    (v: string) => v.length >= 3 || 'Name must be more than 3 characters'
  ]

  private email: string = ''
  private emailRules: any[] = [
    (v: string) => !!v || 'Email is required',
    (v: string) => /.+@.+/.test(v) || 'Email must be valid'
  ]

  private goBack() {
    this.$router.back()
  }

  private validateStep() {
    const form: any = this.$refs.form

    if (form.validate()) {
      this.activeStep++
    }
  }

  private async createProject() {
    const form: any = this.$refs.form

    if (form.validate()) {
      const newProject = await this.projectModule.createProject({
        id: '',
        name: this.name,
        email: this.email
      })

      // TODO: change to project overview route (or first steps to configure the project)
      this.$router.push(`/project/${newProject.id}/calendar`)
    }
  }

}
</script>
