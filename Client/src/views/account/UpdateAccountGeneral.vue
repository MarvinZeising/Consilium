<template>
  <v-container fluid>
    <v-form ref="form">

      <v-flex xs12>
        <h1 class="headline">Update Account General</h1>
      </v-flex>
      <v-flex xs12 sm10 md8 lg6 >
        <p class="mt-4 grey--text text--darken-1">
          Your Email address. This Email is unique to your Account. You use it to sign in to this Account.
          <br>
          After saving, you will be signed out and will have to sign in with the new Email address.
        </p>
        <v-text-field
          v-model="email"
          label="Email address"
          :rules="emailRules"
          box
          required
        />

        <div class="mt-4">
          <v-btn :to="{ name: 'account' }">
            Cancel
          </v-btn>
          <v-btn
            @click="save"
            type="submit"
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
import { getModule } from 'vuex-module-decorators'
import { Project } from '@/models/definitions'
import UserModule from '@/store/modules/users';

@Component
export default class UpdateGeneral extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)

  private userId: string = ''
  private currentEmail: string = ''
  private email: string = ''
  private emailRules: any[] = [
    (v: string) => !!v || 'Email is required',
    (v: string) => /.+@.+/.test(v) || 'Email must be valid'
  ]

  private created() {
    const user = this.userModule.myUser
    if (user) {
      this.userId = user.id
      this.currentEmail = user.email
    }
  }

  private async save() {
    const form: any = this.$refs.form

    if (form.validate()) {
      await this.userModule.updateEmail(this.email)
      // TODO: add error handling

      this.$router.back()
    }
  }
}
</script>
