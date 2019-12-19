<template>
  <v-container fluid fill-height>
    <v-layout align-center justify-center>
      <v-flex xs12 sm10 md8 lg6 xl4>
        <v-card
          class="mx-auto elevation-10"
          max-width="500"
        >
          <v-card-title class="title font-weight-regular justify-space-between">
            <span>{{ currentTitle }}</span>
            <v-avatar
              color="primary lighten-2"
              class="subtitle-1 white--text"
              size="24"
              v-text="step"
              v-if="step > 0 && step < 3"
            />
          </v-card-title>

          <v-window v-model="step">
            <v-window-item :value="0">
              <div class="pa-3 text-xs-center">
                <h2
                  class="headline"
                  v-t="'account.signUp.fullTitle'"
                />
                <p v-t="'account.signUp.description1'" />
                <p v-t="'account.signUp.description2'" />
                <p>
                  {{ $t('account.signUp.hint1') }}
                  <br>
                  {{ $t('account.signUp.hint2') }}
                </p>
                <p v-t="'account.signUp.signInHint'" />
              </div>
            </v-window-item>

            <v-window-item :value="1">
              <v-card-text>

                <v-form
                  ref="emailForm"
                  v-model="emailValid"
                >
                  <p
                    class="grey--text text--darken-1"
                    v-t="'account.emailDescription'"
                  />
                  <v-text-field
                    v-model="email"
                    name="email"
                    type="email"
                    :label="$t('core.email')"
                    :rules="emailRules"
                    filled
                  />
                </v-form>
                <span
                  class="red--text"
                  v-if="emailAlreadyExists"
                  v-t="'account.emailUnique'"
                />

              </v-card-text>
            </v-window-item>

            <v-window-item :value="2">
              <v-card-text>

                <v-form
                  ref="passwordForm"
                  v-model="passwordValid"
                >
                  <p
                    class="grey--text text--darken-1"
                    v-t="'account.signUp.passwordDescription'"
                  />
                  <v-text-field
                    v-model="password"
                    :label="$t('account.password')"
                    :append-icon="passwordShow ? 'visibility' : 'visibility_off'"
                    :type="passwordShow ? 'text' : 'password'"
                    :rules="passwordRules"
                    :hint="$t('account.signUp.passwordRuleHint')"
                    name="password"
                    filled
                    required
                    @click:append="passwordShow = !passwordShow"
                  />
                  <p
                    class="grey--text text--darken-1"
                    v-t="'account.signUp.passwordRepeatDescription'"
                  />
                  <v-text-field
                    v-model="passwordRepeat"
                    :label="$t('account.passwordRepeat')"
                    :append-icon="passwordRepeatShow ? 'visibility' : 'visibility_off'"
                    :type="passwordRepeatShow ? 'text' : 'password'"
                    :rules="passwordRepeatRules"
                    name="passwordRepeat"
                    filled
                    required
                    @click:append="passwordRepeatShow = !passwordRepeatShow"
                  />
                </v-form>

              </v-card-text>
            </v-window-item>

            <v-window-item :value="3">
              <div class="pa-3 text-xs-center">
                <h2
                  class="headline"
                  v-t="'account.signUp.success'"
                />
                <p>
                  {{ $t('account.signUp.successDescription1') }}
                  <br>
                  {{ $t('account.signUp.successDescription2') }}
                  <br>
                  {{ $t('account.signUp.successDescription3') }}
                </p>
              </div>
            </v-window-item>
          </v-window>

          <v-divider />

          <v-card-actions v-if="step < 3">
            <v-btn
              v-if="step === 0"
              text
              :to="{ name: 'signIn' }"
              v-t="'account.signUp.signInLink'"
            />
            <v-btn
              v-if="step !== 0"
              text
              @click="step--"
              v-t="'core.back'"
            />
            <v-spacer />
            <v-btn
              :disabled="(step === 1 && (!emailValid || nextLoading)) || (step === 2 && !passwordValid)"
              color="primary"
              @click="next"
            >
              <span
                v-if="step === 0"
                v-t="'core.start'"
              />
              <span
                v-if="!nextLoading && step > 0 && step < 3"
                v-t="'core.next'"
              />
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
import { Vue, Component } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import colors from 'vuetify/es5/util/colors'
import i18n from '@/i18n'
import UserModule from '@/store/modules/users'

@Component
export default class SignUp extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)

  private step: number = 0
  private nextLoading: boolean = false

  private email: string = ''
  private emailValid: boolean = false
  private emailAlreadyExists: boolean = false
  private emailRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => /.+@.+/.test(v) || i18n.t('core.emailInvalid'),
  ]

  private password: string = ''
  private passwordShow: boolean = false
  private passwordValid: boolean = false
  private passwordRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => v.length >= 8 || i18n.t('core.fieldMin', { count: 8 }),
  ]

  private passwordRepeat: string = ''
  private passwordRepeatShow: boolean = false
  private get passwordRepeatRules() {
    return [
      (v: string) => !!v || i18n.t('core.fieldRequired'),
      (v: string) => v === this.password || i18n.t('account.passwordMustEqual'),
    ]
  }

  private get currentTitle() {
    switch (this.step) {
      case 0: return ''
      case 1: return i18n.t('account.signUp.email')
      case 2: return i18n.t('account.signUp.password')
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
