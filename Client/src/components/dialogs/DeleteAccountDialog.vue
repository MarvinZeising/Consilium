<template>
  <v-layout>
    <v-dialog v-model="deleteAccountDialog" persistent max-width="600px">
      <template v-slot:activator="{ on }">
        <v-btn
          v-on="on"
          color="error"
        >
          Delete my Account
        </v-btn>
      </template>
      <v-card>
        <v-card-title>
          <span class="headline">Delete Account</span>
        </v-card-title>
        <v-card-text>
          <v-form v-model="valid">
            <p class="subheading">
              This will delete your Account and all the Persons linked to that Account. You won't be able to sign in to the Account again after this.
            </p>
            <p class="subheading">
              Enter the Account's Email Address to continue
            </p>
            <v-text-field
              v-model="email"
              label="Email Address"
              :rules="emailRules"
              box
              required
            ></v-text-field>
            <p class="subheading text-uppercase error--text">
              !!! This cannot be undone - everything will be gone !!!
            </p>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            flat
            color="black"
            @click="deleteAccountDialog = false"
          >
            Cancel
          </v-btn>
          <v-btn
            :disabled="!valid"
            flat
            color="error"
            @click="deleteAccount"
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
import axios from 'axios'
import Component from 'vue-class-component'
import ProjectModule from '@/store/modules/projects'
import UserModule from '@/store/modules/users'
import { getModule } from 'vuex-module-decorators'
import { Project } from '../../models/definitions'
import { VForm } from 'vuetify/lib'

@Component
export default class DeleteAccountDialog extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)

  private valid: any = false
  private deleteAccountDialog: boolean = false
  private accountEmail: string = ''
  private email: string = ''
  private get emailRules() {
    return [
      (v: string) => !!v || 'Email is required',
      (v: string) => /.+@.+/.test(v) || 'Email must be valid',
      (v: string) => v === this.accountEmail || 'Email must be your Account\'s Email',
    ]
  }

  private created() {
    const user = this.userModule.myUser
    if (user) {
      this.accountEmail = user.email
    }
  }

  private async deleteAccount() {
    if (this.accountEmail === this.email) {
      await this.userModule.deleteAccount()

      this.deleteAccountDialog = false
      this.$router.push({ name: 'home' })
    } else {
      alert('Wrong Email Address!')
    }
  }

}
</script>
