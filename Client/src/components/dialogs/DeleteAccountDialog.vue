<template>
  <v-layout>
    <v-dialog v-model="deleteAccountDialog" persistent max-width="600px">
      <template v-slot:activator="{ on }">
        <v-btn
          v-on="on"
          color="error"
          v-t="'account.delete'"
        />
      </template>
      <v-card>
        <v-form v-model="valid">
          <v-card-title>
            <span
              class="headline"
              v-t="'account.delete'"
            />
          </v-card-title>
          <v-card-text>
            <p
              class="subheading"
              v-t="'account.deleteDescription'"
            />
            <p
              class="subheading"
              v-t="'account.deleteHint'"
            />
            <v-text-field
              v-model="email"
              :label="$t('core.email')"
              :rules="emailRules"
              box
              required
            />
            <p
              class="subheading text-uppercase error--text"
              v-t="'account.deleteWarning'"
            />
          </v-card-text>
          <v-card-actions>
            <v-spacer />
            <v-btn
              flat
              color="black"
              @click="deleteAccountDialog = false"
              v-t="'core.cancel'"
            />
            <v-btn
              :disabled="!valid"
              type="submit"
              flat
              color="error"
              @click="deleteAccount"
              v-t="'core.delete'"
            />
          </v-card-actions>
        </v-form>
      </v-card>
    </v-dialog>
  </v-layout>
</template>

<script lang="ts">
import Vue from 'vue'
import Component from 'vue-class-component'
import ProjectModule from '@/store/modules/projects'
import UserModule from '@/store/modules/users'
import { getModule } from 'vuex-module-decorators'
import { Project } from '../../models/definitions'
import { VForm } from 'vuetify/lib'
import i18n from '@/i18n'

@Component
export default class DeleteAccountDialog extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)

  private valid: any = false
  private deleteAccountDialog: boolean = false
  private accountEmail: string = ''
  private email: string = ''
  private get emailRules() {
    return [
      (v: string) => !!v || i18n.t('core.fieldRequired'),
      (v: string) => /.+@.+/.test(v) || i18n.t('core.emailInvalid'),
      (v: string) => v === this.accountEmail || i18n.t('account.emailMustEqual')
    ]
  }

  private created() {
    const user = this.userModule.myUser
    if (user) {
      this.accountEmail = user.email
    }
  }

  private async deleteAccount() {
    await this.userModule.deleteAccount()

    this.deleteAccountDialog = false
    this.$router.push({ name: 'home' })
  }

}
</script>
