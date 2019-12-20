<template>
  <v-container fluid>
    <v-form ref="form">

      <v-flex xs12>
        <h1
          class="headline"
          v-t="'account.updateLanguage'"
        />
      </v-flex>

      <v-flex xs12 sm10 md8 lg6 xl4>
        <p
          class="mt-4 grey--text text--darken-1"
          v-t="'account.languageDescription'"
        />
        <p
          class="mt-4 grey--text text--darken-1"
          v-t="'account.languageHint'"
        />
        <v-select
          v-model="language"
          :items="languages"
          :label="$t('account.language')"
          filled
          required
        />

        <div>
          <v-btn
            :to="{ name: 'account' }"
            v-t="'core.cancel'"
          />
          <v-btn
            :disabled="this.language === $i18n.locale"
            @click="save"
            color="primary"
            v-t="'core.save'"
          />
        </div>

        <p class="mt-5 grey--text text--darken-1">
          {{ $t('account.translationHelp') }}
          <a href="mailto:support@consiliumapp.org">support@consiliumapp.org</a>
        </p>
      </v-flex>

    </v-form>
  </v-container>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import { VForm } from 'vuetify/lib'
import { getModule } from 'vuex-module-decorators'
import i18n from '@/i18n'
import UserModule from '@/store/modules/users'
import { Project } from '@/models/definitions'

@Component
export default class UpdateAccountLanguage extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)
  private language: string = ''
  private languages: string[] = []

  private created() {
    this.languages = i18n.availableLocales
    const user = this.userModule.getUser
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
