<template>
  <v-dialog
    v-model="dialog"
    max-width="600px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        class="mb-4"
        v-t="'account.changePassword'"
      />
    </template>
    <v-card>
      <v-form
        v-model="valid"
        ref="form"
      >
        <v-card-title>
          <span
            class="headline"
            v-t="'account.changePassword'"
          />
        </v-card-title>

        <v-card-text>
          <p class="subtitle-1">
            {{ $t('account.changePasswordDescription1') }}
            <br>
            {{ $t('account.changePasswordDescription2') }}
          </p>
          <p
            class="subtitle-1"
            v-t="'account.changePasswordHint'"
          />
          <p
            class="subtitle-1"
            v-t="'account.currentPasswordHint'"
          />
          <v-text-field
            v-model="currentPassword"
            :label="$t('account.currentPassword')"
            :rules="passwordRules"
            :append-icon="currentPasswordShow ? 'visibility' : 'visibility_off'"
            :type="currentPasswordShow ? 'text' : 'password'"
            prepend-inner-icon="lock"
            @click:append="currentPasswordShow = !currentPasswordShow"
            filled
            required
          />
          <p
            class="subtitle-1"
            v-t="'account.newPasswordHint'"
          />
          <v-text-field
            v-model="newPassword"
            :label="$t('account.newPassword')"
            :rules="passwordRules"
            :append-icon="newPasswordShow ? 'visibility' : 'visibility_off'"
            :type="newPasswordShow ? 'text' : 'password'"
            prepend-inner-icon="lock"
            @click:append="newPasswordShow = !newPasswordShow"
            filled
            required
          />
          <v-text-field
            v-model="newPasswordRepeat"
            :label="$t('account.newPasswordRepeat')"
            :rules="passwordRules"
            :append-icon="newPasswordRepeatShow ? 'visibility' : 'visibility_off'"
            :type="newPasswordRepeatShow ? 'text' : 'password'"
            prepend-inner-icon="lock"
            @click:append="newPasswordRepeatShow = !newPasswordRepeatShow"
            filled
            required
          />
        </v-card-text>

        <v-card-actions>
          <v-spacer />
          <v-btn
            text
            @click="dialog = false"
            v-t="'core.cancel'"
          />
          <v-btn
            :disabled="!valid"
            type="submit"
            text
            color="primary"
            @click="save"
            v-t="'core.save'"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import { VForm } from 'vuetify/lib'
import i18n from '../../i18n'
import UserModule from '../../store/users'

@Component
export default class UpdatePasswordDialog extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)

  private dialog: any = false
  private valid: any = null

  private currentPassword: string = ''
  private currentPasswordShow: boolean = false
  private newPassword: string = ''
  private newPasswordShow: boolean = false
  private newPasswordRepeat: string = ''
  private newPasswordRepeatShow: boolean = false
  private passwordRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => v.length >= 8 || i18n.t('core.fieldMin', { count: 8 })
  ]

  private async save() {
    const projectId = this.$route.params.projectId

    if (this.valid) {
      try {
        await this.userModule.updatePassword({
          old: this.currentPassword,
          new: this.newPassword,
        })

        this.dialog = false

        this.$router.push({ name: 'home' })
      } catch (e) {
        const thisPassword = this.currentPassword
        this.passwordRules.push((v: string) => v !== thisPassword || i18n.t('account.passwordWrong'))

        const form: any = this.$refs.form
        form.validate()
      }
    }
  }

}
</script>
