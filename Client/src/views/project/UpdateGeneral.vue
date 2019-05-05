<template>
  <v-container fluid>

    <v-form ref="form">

      <v-flex xs12>
        <h1 class="headline">Update General Information</h1>
      </v-flex>

      <v-flex
        xs12 sm8 md6 lg4
      >
        <p class="mt-4 grey--text text--darken-1">
          The name of this Project. The Name has to be unique accross all Projects.
        </p>
        <v-text-field
          v-model="name"
          label="Project Name"
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
          label="Project Email"
          :rules="emailRules"
          box
          required
        />

        <div class="mt-4">
          <v-btn :to="{ name: 'settings' }">
            Cancel
          </v-btn>

          <v-btn
            @click="save"
            color="primary"
          >
            Save
          </v-btn>
        </div>
      </v-flex>
    </v-form>

  </v-container>
</template>

<script lang="ts">
import Vue from 'vue'
import axios from '@/tools/axios'
import { VForm } from 'vuetify/lib'
import Component from 'vue-class-component'
import ProjectModule from '@/store/modules/projects'
import { getModule } from 'vuex-module-decorators'
import { Project } from '@/models/definitions'

@Component
export default class UpdateGeneral extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

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

  private created() {
    const projectId = this.$route.params.projectId
    const project = this.projectModule.myProjects.filter((x: Project) => x.id === projectId)[0]
    this.name = project.name
    this.email = project.email
  }

  private async save() {
    const form: any = this.$refs.form
    const projectId = this.$route.params.projectId

    if (form.validate()) {
      await this.projectModule.updateProjectGeneral({
        id: projectId,
        name: this.name,
        email: this.email
      })

      this.$router.back()
    }
  }
}
</script>
