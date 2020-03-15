<template>
  <v-flex xs12 sm10 md8 lg6 xl4>
    <v-card v-if="userModule.getUser" flat class="ma-2 mb-5">
      <!-- //* READ -->
      <v-card-text v-if="!editMode" class="text--primary">
        <v-layout wrap>
          <v-flex xs12>
            <h2 class="headline mb-5" v-t="'account.interface'" />
          </v-flex>

          <v-flex xs12 sm6>
            <p class="caption mb-0 grey--text" v-t="'language.language'" />
            <p class="subtitle-1" v-t="'language.' + userModule.getUser.language" />
          </v-flex>

          <v-flex xs12 sm6>
            <p class="caption mb-0 grey--text" v-t="'account.theme'" />
            <p class="subtitle-1">{{ $t(`account.${userModule.getUser.theme}`) }}</p>
          </v-flex>

          <v-flex xs12 sm6>
            <p class="caption mb-0 grey--text" v-t="'account.dateFormat'" />
            <p class="subtitle-1" v-text="getTodayFormatted(userModule.getUser.dateFormat)" />
          </v-flex>

          <v-flex xs12 sm6>
            <p class="caption mb-0 grey--text" v-t="'account.timeFormat'" />
            <p class="subtitle-1" v-text="getTodayFormatted(userModule.getUser.timeFormat)" />
          </v-flex>
        </v-layout>
      </v-card-text>

      <!-- //* UPDATE -->
      <v-card-text v-else>
        <v-form ref="form">
          <p v-t="'account.languageDescription'" />
          <v-select
            v-model="language"
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
          <p class="caption mb-5" style="margin-top:-20px;">
            <i>
              {{ $t('account.translationHelp') }}
              <a
                href="mailto:support@consiliumapp.org"
              >support@consiliumapp.org</a>
            </i>
          </p>

          <p v-t="'account.themeDescription'" />
          <v-select
            v-model="theme"
            :items="themeValues"
            item-text="name"
            item-value="value"
            :label="$t('account.theme')"
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

          <p v-t="'account.dateFormatDescription'" />
          <v-select
            v-model="dateFormat"
            :items="dateFormatValues"
            item-text="name"
            item-value="value"
            :label="$t('account.dateFormat')"
            filled
            required
          >
            <template v-slot:selection="{ item }">
              <span v-text="getTodayFormatted(item.value)" />
            </template>
            <template v-slot:item="{ item }">
              <span v-text="getTodayFormatted(item.value)" />
            </template>
          </v-select>

          <p v-t="'account.timeFormatDescription'" />
          <v-select
            v-model="timeFormat"
            :items="timeFormatValues"
            item-text="name"
            item-value="value"
            :label="$t('account.timeFormat')"
            filled
            required
          >
            <template v-slot:selection="{ item }">
              <span v-text="getTodayFormatted(item.value)" />
            </template>
            <template v-slot:item="{ item }">
              <span v-text="getTodayFormatted(item.value)" />
            </template>
          </v-select>
        </v-form>
      </v-card-text>

      <!-- //* ACTIONS -->
      <v-card-actions>
        <v-spacer />
        <v-btn text v-if="!editMode" @click.stop="toggleEditMode" v-t="'core.edit'" />
        <v-btn text v-if="editMode" @click.stop="toggleEditMode" v-t="'core.cancel'" />
        <v-btn text v-if="editMode" color="primary" @click.stop="save" v-t="'core.save'" />
      </v-card-actions>
    </v-card>
  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import moment from 'moment'
import i18n from '../i18n'
import UserModule from '../store/users'
import { Language } from '../models'

@Component
export default class AccountInterface extends Vue {
  private userModule = getModule(UserModule, this.$store)

  private editMode = false

  private dateFormat: string = this.userModule.getUser?.dateFormat || ''
  private dateFormatValues: any[] = [
    'ddd, MMM Do YYYY',
    'dd, MMM Do YYYY',
    'MMM Do YYYY',
    'YYYY-MM-DD',
    'YYYY-DD-MM',
    'MM-DD-YYYY',
    'DD-MM-YYYY',
    'MM.DD.YYYY',
    'DD.MM.YYYY',
    'D.M.YYYY',
    'D. MMM YYYY',
    'dd, D. MMM YYYY',
    'ddd, D. MMM YYYY',
  ].map((value) => {
    return { value }
  })
  private timeFormat: string = this.userModule.getUser?.timeFormat || ''
  private timeFormatValues: any[] = [
    'h:mm a',
    'h:mm A',
    'H:mm [hours]',
    'h:mm [Uhr]',
    'H:mm [Uhr]',
  ].map((value) => {
    return { value }
  })
  private language: Language = this.userModule.getUser?.language || Language.enUS
  private languageValues: any[] = i18n.availableLocales.map((value) => {
    return { value }
  })
  private theme: string = this.userModule.getUser?.theme || ''
  private themeValues: any[] = ['light', 'dark'].map((value) => {
    return { value }
  })

  private toggleEditMode() {
    this.editMode = !this.editMode

    if (this.editMode) {
      this.language = this.userModule.getUser?.language || Language.enUS
      this.theme = this.userModule.getUser?.theme || ''
      this.dateFormat = this.userModule.getUser?.dateFormat || ''
      this.timeFormat = this.userModule.getUser?.timeFormat || ''
    }
  }

  private getTodayFormatted(format: string) {
    return moment().format(format)
  }

  private async save() {
    const form: any = this.$refs.form
    const userId = this.userModule.getUser?.language || ''

    if (form.validate()) {
      await this.userModule.updateInterface({
        language: this.language,
        theme: this.theme,
        dateFormat: this.dateFormat,
        timeFormat: this.timeFormat,
      })

      this.$vuetify.theme.dark = this.theme === 'dark'

      this.toggleEditMode()
    }
  }
}
</script>
