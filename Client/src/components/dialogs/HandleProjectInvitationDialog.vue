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
        v-t="'project.handleInvitation'"
      />
      <v-card-text v-t="'project.handleInvitationDescription'" />
      <v-card-actions>
        <v-btn
          text
          @click.stop="dialog = false"
          v-t="'core.cancel'"
        />
        <v-spacer></v-spacer>
        <v-btn
          text
          color="error"
          @click.stop="declineInvitation"
          v-t="'core.decline'"
          :loading="loading"
        />
        <v-btn
          text
          color="primary"
          @click.stop="acceptInvitation"
          v-t="'core.accept'"
          :loading="loading"
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
import { Participation } from '../../models/definitions'

@Component
export default class HandleProjectInvitationDialog extends Vue {
  private invitationModule: InvitationModule = getModule(InvitationModule, this.$store)

  @Prop(Participation)
  private readonly participation?: Participation

  private form: any = null
  private dialog: any = null
  private loading: boolean = false

  private async acceptInvitation() {
    this.loading = true

    if (this.participation) {
      await this.invitationModule.acceptInvitation(this.participation)
    }

    this.dialog = false
  }

  private async declineInvitation() {
    this.loading = true

    if (this.participation) {
      await this.invitationModule.declineInvitation(this.participation)
    }

    this.dialog = false
  }

}
</script>
