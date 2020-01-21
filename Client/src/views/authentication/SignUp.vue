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
            <span
              class="subtitle-1"
              v-if="step > 0 && step < 4"
            >
              Step {{step}} of 3
            </span>
          </v-card-title>

          <v-window v-model="step">

            <v-window-item :value="0">
              <v-card-text>
                <p v-t="'account.signUpForm.description1'" />
                <p v-t="'account.signUpForm.description2'" />
                <p>
                  {{ $t('account.signUpForm.hint1') }}
                  <br>
                  {{ $t('account.signUpForm.hint2') }}
                </p>
                <p v-t="'account.signUpForm.signInHint'" />
              </v-card-text>
            </v-window-item>

            <v-window-item :value="1">
              <v-card-text>
                <v-form
                  ref="emailForm"
                  v-model="emailValid"
                >
                  <p v-t="'account.emailDescription'" />
                  <v-text-field
                    v-model="email"
                    name="email"
                    type="email"
                    :label="$t('core.email')"
                    :rules="emailRules"
                    autocomplete="username"
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
                  <p v-t="'account.signUpForm.passwordDescription'" />
                  <v-text-field
                    v-model="password"
                    :label="$t('account.password')"
                    :append-icon="passwordShow ? 'visibility' : 'visibility_off'"
                    :type="passwordShow ? 'text' : 'password'"
                    :rules="passwordRules"
                    :hint="$t('account.signUpForm.passwordRuleHint')"
                    autocomplete="new-password"
                    name="password"
                    filled
                    required
                    @click:append="passwordShow = !passwordShow"
                  />
                  <p v-t="'account.signUpForm.passwordRepeatDescription'" />
                  <v-text-field
                    v-model="passwordRepeat"
                    :label="$t('account.passwordRepeat')"
                    :append-icon="passwordRepeatShow ? 'visibility' : 'visibility_off'"
                    :type="passwordRepeatShow ? 'text' : 'password'"
                    :rules="passwordRepeatRules"
                    name="passwordRepeat"
                    autocomplete="new-password"
                    auto
                    filled
                    required
                    @click:append="passwordRepeatShow = !passwordRepeatShow"
                  />
                </v-form>
              </v-card-text>
            </v-window-item>

            <v-window-item :value="3">
              <v-card-text>
                <v-form
                  ref="interfaceForm"
                  v-model="interfaceValid"
                >
                  <p v-t="'account.signUpForm.interfaceDescription'" />
                  <p v-t="'account.languageDescription'" />
                  <v-select
                    v-model="$i18n.locale"
                    :items="languageValues"
                    item-text="name"
                    item-value="value"
                    :label="$t('language.language')"
                    filled
                    required
                  >
                    <template v-slot:selection="{ item }">
                      <span>{{ $t('language.' + item.value) }}</span>
                    </template>
                    <template v-slot:item="{ item }">
                      <span>{{ $t('language.' + item.value) }}</span>
                    </template>
                  </v-select>
                  <p v-t="'account.themeDescription'" />
                  <v-select
                    v-model="theme"
                    :items="themeValues"
                    item-text="name"
                    item-value="value"
                    :label="$t('account.theme')"
                    @change="toggleTheme"
                    filled
                    required
                  >
                    <template v-slot:selection="{ item }">
                      <span>{{ $t('account.' + item.value) }}</span>
                    </template>
                    <template v-slot:item="{ item }">
                      <span>{{ $t('account.' + item.value) }}</span>
                    </template>
                  </v-select>
                </v-form>
                <span
                  class="red--text"
                  v-if="emailAlreadyExists"
                  v-t="'account.emailUnique'"
                />
              </v-card-text>
            </v-window-item>

            <v-window-item :value="4">
              <div class="pa-3 text-xs-center">
                <p>
                  {{ $t('account.signUpForm.successDescription1') }}
                  <br>
                  {{ $t('account.signUpForm.successDescription2') }}
                  <br>
                  {{ $t('account.signUpForm.successDescription3') }}
                </p>
              </div>
            </v-window-item>

          </v-window>

          <v-divider />

          <v-card-actions v-if="step < 4">
            <v-btn
              v-if="step === 0"
              text
              :to="{ name: 'signIn' }"
              v-t="'account.signUpForm.signInLink'"
            />
            <v-btn
              v-if="step !== 0"
              text
              @click.stop="step--"
              v-t="'core.back'"
            />
            <v-spacer />
            <v-btn
              :disabled="(step === 1 && (!emailValid || nextLoading)) || (step === 2 && !passwordValid) || (step === 3 && !interfaceValid)"
              color="primary"
              @click.stop="next"
              :loading="nextLoading"
            >
              <span
                v-if="step === 0"
                v-t="'core.start'"
              />
              <span
                v-else-if="step < 3"
                v-t="'core.next'"
              />
              <span
                v-else
                v-t="'core.done'"
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
import i18n from '../../i18n'
import UserModule from '../../store/users'

@Component
export default class SignUp extends Vue {
  private userModule = getModule(UserModule, this.$store)

  private step: number = 0
  private nextLoading: boolean = false

  private email: string = ''
  private emailValid: boolean = false
  private emailAlreadyExists: boolean = false
  private emailRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => /.+@.+\..+/.test(v) || i18n.t('core.emailInvalid'),
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

  private interfaceValid: any = false
  private languageValues: any[] = i18n.availableLocales.map((value) => {
    return { value }
  })
  private theme: any = this.$vuetify.theme.dark ? 'dark' : 'light'
  private themeValues: any[] = [ 'light', 'dark' ].map((value) => {
    return { value }
  })

  private get currentTitle() {
    switch (this.step) {
      case 0: return i18n.t('account.signUpForm.title')
      case 1: return i18n.t('account.signUpForm.email')
      case 2: return i18n.t('account.signUpForm.password')
      case 3: return i18n.t('account.signUpForm.interface')
      case 4: return i18n.t('account.signUpForm.success')
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
        this.theme = this.$vuetify.theme.dark ? 'dark' : 'light'
        this.step++
        break
      }
      case 3: {
        await this.userModule.signUp({
          email: this.email,
          password: this.password,
          language: this.$i18n.locale,
          theme: this.theme,
        })
        this.step++
        break
      }
      case 4: {
        this.$router.push({ name: 'signIn' })
        break
      }
    }
  }

  private toggleTheme() {
    this.$vuetify.theme.dark = this.theme === 'dark'
  }

}
</script>
