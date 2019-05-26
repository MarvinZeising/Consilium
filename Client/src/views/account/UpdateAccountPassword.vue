<template>
  <v-container fluid>
    <v-form ref="form">

      <v-flex xs12>
        <h1
          class="headline"
          v-t="'account.changePassword'"
        />
      </v-flex>

      <v-flex xs12 sm10 md8 lg6>
        <p class="mt-4 grey--text text--darken-1">
          {{ $t('account.changePasswordDescription1') }}
          <br>
          {{ $t('account.changePasswordDescription2') }}
        </p>
        <p
          class="mt-4 grey--text text--darken-1"
          v-t="'account.changePasswordHint'"
        />
        <p
          class="mt-4 grey--text text--darken-1"
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
          box
          required
        />

        <p
          class="mt-4 grey--text text--darken-1"
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
          box
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
          box
          required
        />

        <div class="mt-4">
          <v-btn
            :to="{ name: 'account' }"
            v-t="'core.cancel'"
          />
          <v-btn
            @click="save"
            color="primary"
            v-t="'core.save'"
          />
        </div>
      </v-flex>

    </v-form>
  </v-container>
</template>

<script lang="ts">
import Vue from 'vue'
import { VForm } from 'vuetify/lib'
import Component from 'vue-class-component'
import UserModule from '@/store/modules/users'
import { getModule } from 'vuex-module-decorators'
import { Project } from '@/models/definitions'

@Component
export default class UpdateAccountPassword extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)

  private currentPassword: string = ''
  private currentPasswordShow: boolean = false
  private newPassword: string = ''
  private newPasswordShow: boolean = false
  private newPasswordRepeat: string = ''
  private newPasswordRepeatShow: boolean = false
  private passwordRules: any[] = [
    (v: string) => !!v || this.$t('fieldRequired'),
    (v: string) => v.length >= 8 || this.$t('core.fieldMin', { count: 8 })
  ]

  private async save() {
    const form: any = this.$refs.form
    const projectId = this.$route.params.projectId

    if (form.validate()) {
      await this.userModule.updatePassword({
        old: this.currentPassword,
        new: this.newPassword,
      })
      // TODO: add error handling

      this.$router.back()
    }
  }
}
</script>
