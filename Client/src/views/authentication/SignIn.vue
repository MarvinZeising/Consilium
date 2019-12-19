<template>
  <v-container fluid fill-height>
    <v-layout align-center justify-center>
      <v-flex xs12 sm10 md8 lg6 xl4>
        <v-form
          ref="form"
          type="post"
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
                name="email"
                prepend-inner-icon="person"
                filled
                required
              />
              <v-text-field
                v-model="password"
                :label="$t('account.password')"
                :append-icon="passwordShow ? 'visibility' : 'visibility_off'"
                :type="passwordShow ? 'text' : 'password'"
                :rules="passwordRules"
                name="password"
                prepend-inner-icon="lock"
                filled
                required
                @click:append="passwordShow = !passwordShow"
              />

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
              <v-spacer />
              <v-btn
                :disabled="authInProgress"
                :loading="authInProgress"
                color="primary"
                type="submit"
                @click.stop="signIn"
              >
                <span v-t="'account.signIn'" />
              </v-btn>
            </v-card-actions>
          </v-card>

        </v-form>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import { getModule, Action } from 'vuex-module-decorators'
import colors from 'vuetify/es5/util/colors'
import i18n from '@/i18n'
import UserModule from '@/store/modules/users'
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
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => /.+@.+/.test(v) || i18n.t('core.emailInvalid')
  ]

  private password: string = ''
  private passwordShow: boolean = false
  private passwordRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
  ]

  private created() {
    if (this.userModule.isSignedIn) {
      this.$router.replace({ name: 'home' })
    }
  }

  private async signIn(e: any) {
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
