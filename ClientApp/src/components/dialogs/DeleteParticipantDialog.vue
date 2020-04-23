<template>
  <v-dialog v-model="dialog" max-width="500px">
    <template v-slot:activator="{ on }">
      <v-btn v-on="on" text color="error" v-t="'project.participant.delete'" />
    </template>
    <v-card>
      <v-form v-model="valid">
        <v-card-title>
          <span class="headline" v-t="'project.participant.delete'" />
        </v-card-title>
        <v-card-text>
          <p class="subtitle-1" v-t="'project.participant.deleteDescription'" />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn text @click.stop="dialog = false" v-t="'core.cancel'" />
          <v-btn
            :disabled="!valid"
            type="submit"
            text
            color="error"
            @click.prevent="deleteParticipant"
            v-t="'core.delete'"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import { Project } from '../../models'
import ParticipantModule from '../../store/participants'

@Component
export default class DeleteRoleDialog extends Vue {
  private participantModule = getModule(ParticipantModule, this.$store)

  @Prop(String)
  private readonly participationId?: string

  private valid = false
  private dialog = false
  private loading = false

  private async deleteParticipant() {
    if (this.participationId) {
      this.loading = true

      await this.participantModule.deleteParticipant(this.participationId)
    }

    this.dialog = false
  }
}
</script>
