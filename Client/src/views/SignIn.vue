<template>
  <v-container fluid fill-height>
    <v-layout align-center justify-center>
      <v-flex xs12 sm8 md6 lg4>
        <v-card
          class="mx-auto elevation-10"
          max-width="500"
        >
          <v-card-title class="title font-weight-regular justify-space-between">
            <span>Sign in to Consilium</span>
          </v-card-title>

          <v-card-text>
            <v-form
              ref="form"
              v-model="valid"
            >
              <v-text-field
                v-model="username"
                label="Username"
                :rules="usernameRules"
                prepend-inner-icon="person"
                box
                required
              ></v-text-field>
              <v-text-field
                v-model="password"
                label="Password"
                :append-icon="passwordShow ? 'visibility' : 'visibility_off'"
                :type="passwordShow ? 'text' : 'password'"
                :rules="passwordRules"
                prepend-inner-icon="lock"
                box
                required
                @click:append="passwordShow = !passwordShow"
              ></v-text-field>
            </v-form>
          </v-card-text>

          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn
              color="primary"
            >
              <span @click="signIn">
                Sign in
              </span>
              <v-progress-circular
                v-if="authInProgress"
                indeterminate
                color="primary"
              />
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script lang="ts">
import Vue from 'vue'
import axios from '@/tools/axios'
import Component from 'vue-class-component';
import colors from 'vuetify/es5/util/colors'
import UserModule from '@/store/modules/users';
import { getModule } from 'vuex-module-decorators';

@Component
export default class SignIn extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)

  private valid: boolean = false
  private authInProgress: boolean = false

  private username: string = ''
  private usernameRules: any[] = [
    (v: string) => !!v || 'Username is required'
  ]

  private password: string = ''
  private passwordShow: boolean = false
  private passwordRules: any[] = [
    (v: string) => !!v || 'Password is required'
  ]

  private async signIn() {
    this.authInProgress = true

    const form: any = this.$refs.form

    if (form.validate()) {
      await this.userModule.signIn(this.username, this.password)
      this.$router.push({ name: 'home' })
    }

    this.authInProgress = false
  }
}
</script>
