<template>
  <v-dialog v-model="dialog" max-width="400px">
    <template v-slot:activator="{ on }">
      <v-btn v-on="on" text color="error" v-t="'core.delete'" />
    </template>
    <v-card>
      <v-form v-model="valid">
        <v-card-title>
          <span class="headline" v-t="'project.invitation.delete'" />
        </v-card-title>
        <v-card-text>
          <p class="subtitle-1" v-t="'project.invitation.deleteDescription'" />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn text @click.stop="dialog = false" v-t="'core.cancel'" />
          <v-btn
            :disabled="!valid"
            :loading="loading"
            type="submit"
            text
            color="error"
            @click.prevent="deleteInvitation"
            v-t="'core.delete'"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import i18n from '../../i18n'
import InvitationModule from '../../store/invitations'
import { getModule } from 'vuex-module-decorators'
import { Project } from '../../models'

@Component
export default class DeleteInvitationDialog extends Vue {
  private invitationModule = getModule(InvitationModule, this.$store)

  @Prop(String)
  private readonly participationId?: string

  private valid = false
  private dialog = false
  private loading = false

  private async deleteInvitation() {
    if (this.participationId) {
      this.loading = true

      await this.invitationModule.cancelInvitation(this.participationId)
    }

    this.dialog = false
  }
}
</script>
