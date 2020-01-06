<template>
  <v-dialog
    v-model="dialog"
    max-width="400px"
  >
    <template v-slot:activator="{ on }">
      <v-icon v-on="on">delete</v-icon>
    </template>
    <v-card>
      <v-form v-model="valid">
        <v-card-title>
          <span
            class="headline"
            v-t="'project.participant.delete'"
          />
        </v-card-title>
        <v-card-text>
          <p
            class="subtitle-1"
            v-t="'project.participant.deleteDescription'"
          />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn
            text
            @click.stop="dialog = false"
            v-t="'core.cancel'"
          />
          <v-btn
            :disabled="!valid"
            type="submit"
            text
            color="error"
            @click.stop="deleteParticipant"
            v-t="'core.delete'"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import { VForm } from 'vuetify/lib'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import { Project } from '../../models/definitions'
import ParticipantModule from '../../store/participants'

@Component
export default class DeleteRoleDialog extends Vue {
  private participantModule: ParticipantModule = getModule(ParticipantModule, this.$store)

  @Prop(String)
  private readonly participationId?: string

  private valid: any = false
  private dialog: boolean = false
  private loading: boolean = false

  private async deleteParticipant() {
    if (this.participationId) {
      this.loading = true

      await this.participantModule.deleteParticipant(this.participationId)
    }

    this.dialog = false
  }

}
</script>
