<template>
  <v-dialog v-model="dialog" max-width="500px">
    <template v-slot:activator="{ on }">
      <v-btn v-on="on" text color="error" v-t="'project.participation.delete'" :loading="loading" />
    </template>
    <v-card>
      <v-form v-model="valid">
        <v-card-title>
          <span class="headline" v-t="'project.participation.delete'" />
        </v-card-title>
        <v-card-text>
          <p class="subtitle-1" v-t="'project.participation.deleteDescription'" />
          <p v-t="'project.participation.deleteHint'" />
          <v-text-field
            v-model="enteredName"
            :label="$t('core.name')"
            :rules="enteredNameRules"
            filled
            required
          />
        </v-card-text>
        <v-card-text class="error">
          <span class="subtitle-1" v-t="'project.participation.deleteWarning'" />
        </v-card-text>
        <v-card-actions>
          <v-btn text @click.stop="dialog = false" v-t="'core.cancel'" />
          <v-spacer />
          <v-btn
            :disabled="!valid"
            :loading="loading"
            type="submit"
            text
            color="error"
            @click.prevent="deleteParticipation"
            v-t="'project.participation.delete'"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import i18n from '../../i18n'
import ParticipantModule from '../../store/participants'
import { getModule } from 'vuex-module-decorators'
import { Project, Participation } from '../../models'

@Component
export default class DeleteParticipationDialog extends Vue {
  private participantModule = getModule(ParticipantModule, this.$store)

  @Prop(Participation)
  private readonly participation?: Participation

  private valid = false
  private dialog = false
  private loading = false

  private enteredName: string = ''
  private get enteredNameRules() {
    return [
      (v: string) => !!v || i18n.t('core.fieldRequired'),
      (v: string) => v === this.participation?.project?.name || i18n.t('project.nameMustEqual'),
    ]
  }

  private async deleteParticipation() {
    this.loading = true

    if (this.participation) {
      await this.participantModule.deleteParticipation(this.participation)
    }

    this.loading = false
    this.dialog = false
  }
}
</script>
