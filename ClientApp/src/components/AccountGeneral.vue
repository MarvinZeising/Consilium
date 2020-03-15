<template>
  <v-flex xs12 sm10 md8 lg6 xl4>
    <v-card v-if="userModule.getUser" flat class="ma-2 mb-5">
      <!-- //* READ -->
      <v-card-text v-if="!editMode" class="text--primary">
        <v-layout wrap>
          <v-flex xs12>
            <h2 class="headline mb-5" v-t="'core.general'" />
          </v-flex>

          <v-flex xs12>
            <p class="caption mb-0 grey--text" v-t="'core.id'" />
            <p class="subtitle-1 grey--text">{{ userModule.getUser.id }}</p>
          </v-flex>

          <v-flex xs12>
            <p class="caption mb-0 grey--text" v-t="'core.email'" />
            <p class="subtitle-1">{{ userModule.getUser.email }}</p>
          </v-flex>

          <v-flex xs12 sm6>
            <p class="caption mb-0 grey--text" v-t="'core.createdTime'" />
            <p
              class="subtitle-1 grey--text"
            >{{ userModule.getUser.formatDateTime(userModule.getUser.createdTime) }}</p>
          </v-flex>

          <v-flex xs12 sm6>
            <p class="caption mb-0 grey--text" v-t="'core.lastUpdatedTime'" />
            <p
              class="subtitle-1 grey--text"
            >{{ userModule.getUser.formatDateTime(userModule.getUser.lastUpdatedTime) }}</p>
          </v-flex>
        </v-layout>
      </v-card-text>

      <!-- //* UPDATE -->
      <v-card-text v-else>
        <v-form ref="form">
          <p v-t="'account.emailDescription'" />
          <v-text-field
            v-model="email"
            :label="$t('core.email')"
            :rules="emailRules"
            filled
            required
          />

          <v-row no-gutters class="d-flex align-center">
            <v-col cols="1" class="flex-grow-0 flex-shrink-1">
              <v-icon color="warning">warning</v-icon>
            </v-col>
            <v-col cols="11" class="flex-grow-1 flex-shrink-0">
              <span v-t="'account.emailChangeDescription'" />
            </v-col>
          </v-row>
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
import i18n from '../i18n'
import UserModule from '../store/users'

@Component
export default class AccountGeneral extends Vue {
  private userModule = getModule(UserModule, this.$store)

  private editMode = false

  private id: string = this.userModule.getUser?.id || ''
  private email: string = this.userModule.getUser?.email || ''
  private emailRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => /.+@.+\..+/.test(v) || i18n.t('core.emailInvalid'),
  ]

  private toggleEditMode() {
    this.editMode = !this.editMode

    if (this.editMode) {
      this.id = this.userModule.getUser?.id || ''
      this.email = this.userModule.getUser?.email || ''
    }
  }

  private async save() {
    const form: any = this.$refs.form
    const userId = this.userModule.getUser?.id || ''

    if (form.validate()) {
      try {
        await this.userModule.updateGeneral(this.email)
        // TODO: check that email is correct
        // TODO: maybe by sending an email to verify the new one

        this.$router.push({ name: 'signIn' })
      } catch (e) {
        const thisEmail = this.email
        this.emailRules.push((v: string) => v !== thisEmail || i18n.t('account.emailUnique'))
        form.validate()
      }
    }
  }
}
</script>
