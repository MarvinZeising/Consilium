<template>
  <v-container fluid fill-height>
    <v-layout align-center justify-center>
      <v-flex xs12 sm8 md6 lg4>
        <v-form
          ref="form"
          v-model="valid"
        >

          <v-card
            class="mx-auto elevation-10"
            max-width="500"
          >
            <v-card-title class="title font-weight-regular justify-space-between">
              <span v-t="'account.signInFullTitle'" />
            </v-card-title>

            <v-card-text>
              <v-text-field
                v-model="email"
                :label="$t('core.email')"
                :rules="emailRules"
                prepend-inner-icon="person"
                box
                required
              ></v-text-field>
              <v-text-field
                v-model="password"
                :label="$t('account.password')"
                :append-icon="passwordShow ? 'visibility' : 'visibility_off'"
                :type="passwordShow ? 'text' : 'password'"
                :rules="passwordRules"
                prepend-inner-icon="lock"
                box
                required
                @click:append="passwordShow = !passwordShow"
              ></v-text-field>

              <span
                class="red--text"
                v-if="authFailed"
              >
                {{ $t('account.passwordWrong') }}
                <br>
                {{ $t('account.passwordCapslock') }}
              </span>
            </v-card-text>

            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn
                color="primary"
                type="submit"
                @click="signIn"
              >
                <span v-t="'account.signIn'" />
                <v-progress-circular
                  v-if="authInProgress"
                  indeterminate
                  color="white"
                  class="ml-3"
                />
              </v-btn>
            </v-card-actions>
          </v-card>

        </v-form>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script lang="ts">
import Vue from 'vue'
import Component from 'vue-class-component'
import colors from 'vuetify/es5/util/colors'
import UserModule from '@/store/modules/users'
import { getModule, Action } from 'vuex-module-decorators'
import ProjectModule from '@/store/modules/projects'

@Component
export default class SignIn extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  private valid: boolean = false
  private authInProgress: boolean = false
  private authFailed: boolean = false

  private email: string = ''
  private emailRules: any[] = [
    (v: string) => !!v || 'Email is required',
    (v: string) => /.+@.+/.test(v) || 'Email must be valid'
  ]

  private password: string = ''
  private passwordShow: boolean = false
  private passwordRules: any[] = [
    (v: string) => !!v || 'Password is required'
  ]

  private created() {
    if (this.userModule.isSignedIn) {
      this.$router.replace({ name: 'home' })
    }
  }

  private async signIn() {
    this.authInProgress = true
    this.authFailed = false

    const form: any = this.$refs.form

    if (form.validate()) {
      const signInSuccessful = await this.userModule.signIn({
        email: this.email,
        password: this.password
      })

      if (signInSuccessful) {
        const afterSignIn: any = this.$router.currentRoute.query.afterSignIn

        if (afterSignIn) {
          const location = this.$router.resolve(afterSignIn)

          if (location !== undefined) {
            this.$router.push({
              name: location.resolved.name,
              params: location.resolved.params,
              query: location.resolved.query,
            })

            return
          }
        }

        this.$router.push({ name: 'home' })
      } else {
        this.authFailed = true
      }
    }

    this.authInProgress = false
  }
}
</script>
