<template>
  <v-container fluid fill-height>
    <v-layout align-center justify-center>
      <v-flex xs12 sm8 md6 lg4>
        <v-card
          class="mx-auto elevation-10"
          max-width="500"
        >
          <v-card-title class="title font-weight-regular justify-space-between">
            <span>{{ currentTitle }}</span>
            <v-avatar
              color="primary lighten-2"
              class="subheading white--text"
              size="24"
              v-text="step"
            ></v-avatar>
          </v-card-title>

          <v-window v-model="step">
            <v-window-item :value="1">
              <v-card-text>

                <v-form
                  ref="usernameForm"
                  v-model="usernameValid"
                >
                  <p class="grey--text text--darken-1">
                    This is the username you will use to sign in to your Consilium account
                  </p>
                  <v-text-field
                    v-model="username"
                    label="Username"
                    :rules="usernameRules"
                    box
                  />
                </v-form>
                <span
                  class="red--text"
                  v-if="usernameAlreadyExists"
                >
                  Sorry, this username already exists. Please choose another one.
                </span>

              </v-card-text>
            </v-window-item>

            <v-window-item :value="2">
              <v-card-text>

                <v-form
                  ref="passwordForm"
                  v-model="passwordValid"
                >
                  <p class="grey--text text--darken-1">
                    Please choose a password for your account
                  </p>
                  <v-text-field
                    v-model="password"
                    label="Password"
                    :append-icon="passwordShow ? 'visibility' : 'visibility_off'"
                    :type="passwordShow ? 'text' : 'password'"
                    :rules="passwordRules"
                    hint="Has to have at least 8 characters"
                    box
                    required
                    @click:append="passwordShow = !passwordShow"
                  ></v-text-field>
                  <p class="grey--text text--darken-1">
                    Just to be sure, please confirm your password one more time
                  </p>
                  <v-text-field
                    v-model="passwordRepeat"
                    label="Confirm Password"
                    :append-icon="passwordRepeatShow ? 'visibility' : 'visibility_off'"
                    :type="passwordRepeatShow ? 'text' : 'password'"
                    :rules="passwordRepeatRules"
                    box
                    required
                    @click:append="passwordRepeatShow = !passwordRepeatShow"
                  ></v-text-field>
                </v-form>

              </v-card-text>
            </v-window-item>

            <v-window-item :value="3">
              <div class="pa-3 text-xs-center">
                <h2 class="headline">Welcome to Consilium!</h2>
                <p>You successfully created an account.</p>
                <!--// TODO: add link here -->
                <p>If you want some tips and tricks on how to get started, you can have a look at our wiki (link here)</p>
                <p>Otherwise, you can now sign in for the first time.</p>
              </div>
            </v-window-item>
          </v-window>

          <v-divider></v-divider>

          <v-card-actions>
            <v-btn
              :disabled="step === 1"
              flat
              @click="step--"
            >
              Back
            </v-btn>
            <v-spacer></v-spacer>
            <v-btn
              :disabled="(step === 1 && (!usernameValid || nextLoading)) || (step === 2 && !passwordValid)"
              color="primary"
              @click="next"
            >
              <span v-if="!nextLoading && step < 3">
                Next
              </span>
              <span v-if="step === 3">
                Let's go!
              </span>
              <v-progress-circular
                v-if="nextLoading"
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

@Component
export default class SignUp extends Vue {
  private step: number = 1
  private nextLoading: boolean = false

  private username: string = ''
  private usernameValid: boolean = false
  private usernameAlreadyExists: boolean = false
  private usernameRules: any[] = [
    (v: string) => !!v || 'Username is required',
    (v: string) => v.length >= 3 || 'Username must be more than 3 characters'
  ]

  private password: string = ''
  private passwordShow: boolean = false
  private passwordValid: boolean = false
  private passwordRules: any[] = [
    (v: string) => !!v || 'Password is required',
    (v: string) => v.length >= 8 || 'Password must have at least 8 characters'
  ]

  private passwordRepeat: string = ''
  private passwordRepeatShow: boolean = false
  private get passwordRepeatRules() {
    return [
      (v: string) => !!v || 'Confirm password is required',
      (v: string) => v === this.password || 'The passwords have to be equal'
    ]
  }

  private get currentTitle() {
    switch (this.step) {
      case 1: return 'Sign-up'
      case 2: return 'Create a password'
      default: return ''
    }
  }

  private async next() {
    switch (this.step) {
      case 1: {
        this.nextLoading = true

        await axios.get(`/users?username=${this.username}`)
          .then((response) => {
            this.nextLoading = false

            if (response.data.length === 0) {
              this.usernameAlreadyExists = false
              this.step++
            } else {
              this.usernameAlreadyExists = true
            }
          })

        break
      }
      case 2: {
        const form: any = this.$refs.passwordForm

        if (form.validate()) {
          await axios.post('/users', {
            username: this.username,
            password: this.password
          })
          this.step++
        }
        break
      }
      case 3: {
        this.$router.push({ name: 'signIn' })
        break
      }
    }
  }

}
</script>
