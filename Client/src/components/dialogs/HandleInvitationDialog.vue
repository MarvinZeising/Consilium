<template>
  <v-dialog
    v-model="dialog"
    max-width="400px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        text
        class="mt-2"
        v-t="'core.handle'"
      />
    </template>
    <v-card>
      <v-card-title
        class="headline"
        v-t="'project.invitation.handle'"
      />
      <v-card-text v-t="'project.invitation.handleDescription'" />
      <v-card-actions>
        <v-btn
          text
          @click.stop="dialog = false"
          v-t="'core.cancel'"
        />
        <v-spacer />
        <v-btn
          text
          color="error"
          @click.stop="declineInvitation"
          v-t="'core.decline'"
          :loading="declining"
          :disabled="accepting"
        />
        <v-btn
          text
          color="primary"
          @click.stop="acceptInvitation"
          v-t="'core.accept'"
          :loading="accepting"
          :disabled="declining"
        />
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { getModule } from 'vuex-module-decorators'
import { Vue, Component, Prop } from 'vue-property-decorator'
import i18n from '../../i18n'
import InvitationModule from '../../store/invitations'
import { Participation } from '../../models'

@Component
export default class HandleInvitationDialog extends Vue {
  private invitationModule: InvitationModule = getModule(InvitationModule, this.$store)

  @Prop(Participation)
  private readonly participation?: Participation

  private form: any = null
  private dialog: any = null
  private accepting: boolean = false
  private declining: boolean = false

  private async acceptInvitation() {
    this.accepting = true

    if (this.participation) {
      await this.invitationModule.acceptInvitation(this.participation)
    }

    this.accepting = false
    this.dialog = false
  }

  private async declineInvitation() {
    this.declining = true

    if (this.participation) {
      await this.invitationModule.declineInvitation(this.participation)
    }

    this.declining = false
    this.dialog = false
  }

}
</script>
