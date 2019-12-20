<template>
  <v-container fluid>
    <v-form ref="form">

      <v-flex xs12>
        <h1
          class="headline"
          v-t="'account.updateGeneral'"
        />
      </v-flex>
      <v-flex xs12 sm10 md8 lg6 xl4>
        <p class="mt-4 grey--text text--darken-1"
          v-t="'account.emailDescription'"
        />
        <p
          class="mt-4 grey--text text--darken-1"
          v-t="'account.emailChangeDescription'"
        />
        <v-text-field
          v-model="email"
          :label="$t('core.email')"
          :rules="emailRules"
          filled
          required
        />

        <div class="mt-4">
          <v-btn
            :to="{ name: 'account' }"
            v-t="'core.cancel'"
          />
          <v-btn
            @click="save"
            type="submit"
            color="primary"
            v-t="'core.save'"
          />
        </div>
      </v-flex>

    </v-form>
  </v-container>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import { VForm } from 'vuetify/lib'
import { getModule } from 'vuex-module-decorators'
import i18n from '@/i18n'
import { Project } from '@/models/definitions'
import UserModule from '@/store/modules/users'

@Component
export default class UpdateAccountGeneral extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)

  private userId: string = ''
  private currentEmail: string = ''
  private email: string = ''
  private emailRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => /.+@.+/.test(v) || i18n.t('core.emailInvalid')
  ]

  private created() {
    const user = this.userModule.getUser
    if (user) {
      this.userId = user.id
      this.currentEmail = user.email
    }
  }

  private async save() {
    const form: any = this.$refs.form

    if (form.validate()) {
      await this.userModule.updateEmail(this.email)
      // TODO: check that email is correct
      // TODO: maybe by sending an email to verify the new one

      this.$router.back()
    }
  }
}
</script>
