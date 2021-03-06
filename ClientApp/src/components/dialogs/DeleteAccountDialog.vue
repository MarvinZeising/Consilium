<template>
  <v-dialog v-model="deleteAccountDialog" max-width="600px">
    <template v-slot:activator="{ on }">
      <v-btn v-on="on" color="error" v-t="'account.delete'" @click="opened" />
    </template>
    <v-card>
      <v-form v-model="valid">
        <v-card-title>
          <span class="headline" v-t="'account.delete'" />
        </v-card-title>
        <v-card-text>
          <p class="subtitle-1" v-t="'account.deleteDescription'" />
          <p class="subtitle-1" v-t="'account.deleteHint'" />
          <v-text-field
            v-model="email"
            :label="$t('core.email')"
            :rules="emailRules"
            filled
            required
          />
          <p class="subtitle-1 text-uppercase error--text" v-t="'account.deleteWarning'" />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn text @click.stop="deleteAccountDialog = false" v-t="'core.cancel'" />
          <v-btn
            :disabled="!valid"
            :loading="loading"
            type="submit"
            text
            color="error"
            @click.prevent="deleteAccount"
            v-t="'core.delete'"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import ProjectModule from '../../store/projects'
import UserModule from '../../store/users'
import { getModule } from 'vuex-module-decorators'
import { Project } from '../../models'
import i18n from '../../i18n'

@Component
export default class DeleteAccountDialog extends Vue {
  private userModule = getModule(UserModule, this.$store)

  private valid = false
  private loading = false
  private deleteAccountDialog = false
  private accountEmail: string = ''
  private email: string = ''
  private get emailRules() {
    return [
      (v: string) => !!v || i18n.t('core.fieldRequired'),
      (v: string) => /.+@.+\..+/.test(v) || i18n.t('core.emailInvalid'),
      (v: string) => v === this.accountEmail || i18n.t('account.emailMustEqual'),
    ]
  }

  private opened() {
    const user = this.userModule.getUser
    if (user) {
      this.accountEmail = user.email
    }
  }

  private async deleteAccount() {
    this.loading = true

    await this.userModule.deleteAccount()

    this.deleteAccountDialog = false
    this.$router.push({ name: 'home' })
  }
}
</script>
