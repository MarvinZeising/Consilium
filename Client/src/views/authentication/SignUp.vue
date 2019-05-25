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
              v-if="step > 0 && step < 3"
            />
          </v-card-title>

          <v-window v-model="step">
            <v-window-item :value="0">
              <div class="pa-3 text-xs-center">
                <h2 class="headline">Welcome to Consilium!</h2>
                <p>You want to create an account?</p>
                <p>Perfect! Just click Start.</p>
                <p>
                  You'll be asked to enter your Email Address and choose a password.
                  <br>
                  We'll then send you a verification email to activate your account.
                </p>
                <p>If you already have an account, you can sign in instead.</p>
              </div>
            </v-window-item>

            <v-window-item :value="1">
              <v-card-text>

                <v-form
                  ref="emailForm"
                  v-model="emailValid"
                >
                  <p class="grey--text text--darken-1">
                    This is the email you will use to sign in to your Consilium account
                  </p>
                  <v-text-field
                    v-model="email"
                    type="email"
                    label="Email"
                    :rules="emailRules"
                    box
                  />
                </v-form>
                <span
                  class="red--text"
                  v-if="emailAlreadyExists"
                >
                  Sorry, this email already exists. Please choose another one.
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
                  />
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
                  />
                </v-form>

              </v-card-text>
            </v-window-item>

            <v-window-item :value="3">
              <div class="pa-3 text-xs-center">
                <h2 class="headline">Success!</h2>
                <!--// TODO: add link here -->
                <p>
                  We sent you an email with a verfication link.
                  <br>
                  Use that link to activate your account.
                  <br>
                  Please also check your spam folder if you can't find it.
                </p>
              </div>
            </v-window-item>
          </v-window>

          <v-divider />

          <v-card-actions v-if="step < 3">
            <v-btn
              v-if="step === 0"
              flat
              :to="{ name: 'signIn' }"
            >
              I have an account
            </v-btn>
            <v-btn
              v-if="step !== 0"
              flat
              @click="step--"
            >
              Back
            </v-btn>
            <v-spacer />
            <v-btn
              :disabled="(step === 1 && (!emailValid || nextLoading)) || (step === 2 && !passwordValid)"
              color="primary"
              @click="next"
            >
              <span v-if="step === 0">
                Start
              </span>
              <span v-if="!nextLoading && step > 0 && step < 3">
                Next
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
import Component from 'vue-class-component'
import colors from 'vuetify/es5/util/colors'
import UserModule from '@/store/modules/users'
import { getModule } from 'vuex-module-decorators'

@Component
export default class SignUp extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)

  private step: number = 0
  private nextLoading: boolean = false

  private email: string = ''
  private emailValid: boolean = false
  private emailAlreadyExists: boolean = false
  private emailRules: any[] = [
    (v: string) => !!v || 'Email is required',
    (v: string) => /.+@.+/.test(v) || 'Email must be valid'
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
      case 0: return ''
      case 1: return 'Enter your Email Address'
      case 2: return 'Choose a password'
      default: return ''
    }
  }

  private async next() {
    switch (this.step) {
      case 0: {
        this.step++
        break
      }
      case 1: {
        this.nextLoading = true
        const isEmailAvailable = await this.userModule.isEmailAvailable(this.email)
        this.nextLoading = false

        if (isEmailAvailable) {
          this.emailAlreadyExists = false
          this.step++
        } else {
          this.emailAlreadyExists = true
        }

        break
      }
      case 2: {
        const form: any = this.$refs.passwordForm

        if (form.validate()) {
          await this.userModule.signUp({
            email: this.email,
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
