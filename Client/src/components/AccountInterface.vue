<template>

  <v-flex xs12 sm10 md8 lg6 xl4>
    <h2
      class="headline mb-3"
      v-t="'account.interface'"
    />
    <v-card
      v-if="userModule.getUser"
      flat
      class="ma-2 mb-5"
    >

      <!-- //* READ -->
      <v-card-text
        v-if="!editMode"
        class="text--primary"
      >
        <v-layout wrap>

          <v-flex xs6>
            <p
              class="caption mb-0 grey--text"
              v-t="'language.language'"
            />
            <p
              class="subtitle-1"
              v-t="'language.' + userModule.getUser.language"
            />
          </v-flex>

          <v-flex xs6>
            <p
              class="caption mb-0 grey--text"
              v-t="'account.theme'"
            />
            <p class="subtitle-1">{{ $t(`account.${userModule.getUser.theme}`) }}</p>
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
              <a href="mailto:support@consiliumapp.org">support@consiliumapp.org</a>
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

        </v-form>
      </v-card-text>

      <!-- //* ACTIONS -->
      <v-card-actions>
        <v-spacer />
        <v-btn
          text
          v-if="!editMode"
          @click="toggleEditMode"
          v-t="'core.edit'"
        />
        <v-btn
          text
          v-if="editMode"
          @click="toggleEditMode"
          v-t="'core.cancel'"
        />
        <v-btn
          text
          v-if="editMode"
          color="primary"
          @click="save"
          v-t="'core.save'"
        />
      </v-card-actions>
    </v-card>
  </v-flex>

</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import { VForm } from 'vuetify/lib'
import i18n from '../i18n'
import UserModule from '../store/users'

@Component
export default class AccountInterface extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)

  private editMode: boolean = false

  private language: string = this.userModule.getUser?.language || ''
  private languageValues: any[] = i18n.availableLocales.map((value) => {
    return { value }
  })
  private theme: string = this.userModule.getUser?.theme || ''
  private themeValues: any[] = [ 'light', 'dark' ].map((value) => {
    return { value }
  })

  private toggleEditMode() {
    this.editMode = !this.editMode

    if (this.editMode) {
      this.language = this.userModule.getUser?.language || ''
      this.theme = this.userModule.getUser?.theme || ''
    }
  }

  private async save() {
    const form: any = this.$refs.form
    const userId = this.userModule.getUser?.language || ''

    if (form.validate()) {
      await this.userModule.updateInterface({
        language: this.language,
        theme: this.theme,
      })

      this.$vuetify.theme.dark = this.theme === 'dark'

      this.toggleEditMode()
    }
  }
}
</script>
