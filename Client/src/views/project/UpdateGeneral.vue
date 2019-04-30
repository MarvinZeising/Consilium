<template>
  <v-container fluid>

    <v-form
      v-if="!loading"
      ref="form"
    >
      <v-flex
        v-if="loading"
        class="mb-5"
      >
        <v-progress-circular
          indeterminate
          color="primary"
        />
      </v-flex>

      <v-flex xs12>
        <h1 class="headline">Update General Information</h1>
      </v-flex>

      <v-flex
        xs12 sm8 md6 lg4
      >
        <p class="caption mt-4 grey--text">
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

        <p class="caption mt-4 grey--text">
          We'll use this Email address as reply-to in all Emails that we send on behalf of this Project.
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
export default Vue.extend({
  data() {
    return {
      loading: false,
      name: '',
      nameRules: [
        (v: string) => !!v || 'Name is required',
        (v: string) => v.length <= 40 || 'Name must be less than 40 characters',
        (v: string) => v.length >= 3 || 'Name must be more than 3 characters'
      ],
      email: '',
      emailRules: [
        (v: string) => !!v || 'Email is required',
        (v: string) => /.+@.+/.test(v) || 'Email must be valid'
      ]
    }
  },
  created() {
    this.fetchProject()
  },
  methods: {
    save() {
      const form: any = this.$refs.form
      const projectId = this.$route.params.projectId

      if (form.validate()) {
        axios.put(`/projects/${projectId}`, {
          name: this.name,
          email: this.email
        })
          .then((result) => {
            this.loading = false
            this.$router.back()
            // TODO: toast for successful save
          })
          .catch((err) => {
            this.loading = false
            alert('Couldn\'t reach the server.')
          })
      }
    },
    fetchProject() {
      this.loading = true
      const projectId = this.$route.params.projectId

      axios.get(`/projects/${projectId}`)
        .then((result) => {
          this.loading = false

          if (result.data.case === 'Some') {
            this.name = result.data.fields[0].name
            this.email = result.data.fields[0].email
          } else {
            alert('Couldn\'t find the project.')
          }
        })
        .catch((err) => {
          this.loading = false
          alert('Couldn\'t reach the server.')
        })
    }
  }
})
</script>
