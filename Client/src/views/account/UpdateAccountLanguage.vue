<template>
  <v-container fluid>
    <v-form ref="form">

      <v-flex xs12>
        <h1 class="headline">Update interface language</h1>
      </v-flex>

      <v-flex xs12 sm10 md8 lg6>
        <p class="mt-4 grey--text text--darken-1">
          The language you see this website in.
          <br>
          You can choose one the translated languages.
        </p>
        <p class="mt-4 grey--text text--darken-1">
          If you want to help translating the site into a different language, feel free to reach out to us via
          <a href="mailto:support@consiliumapp.org">support@consiliumapp.org</a>
        </p>
        <p class="mt-4 grey--text text--darken-1">
          Enter your new Password
        </p>
        <v-select
          v-model="language"
          :items="languages"
          label="Interface language"
          box
          required
        />

        <div class="mt-4">
          <v-btn :to="{ name: 'account' }">
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
import axios from 'axios'
import { VForm } from 'vuetify/lib'
import Component from 'vue-class-component'
import UserModule from '@/store/modules/users'
import { getModule } from 'vuex-module-decorators'
import { Project } from '@/models/definitions'

@Component
export default class UpdateAccountLanguage extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)

  private language: string = 'en-US'
  private languages: string[] = ['en-US', 'de-DE']

  private created() {
    const user = this.userModule.myUser
    if (user) {
      this.language = user.language
    }
  }

  private async save() {
    const form: any = this.$refs.form

    if (form.validate()) {
      await this.userModule.updateLanguage(this.language)
      // TODO: add error handling

      this.$router.back()
    }
  }
}
</script>
