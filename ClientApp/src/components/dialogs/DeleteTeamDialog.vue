<template>
  <v-dialog v-model="dialog" max-width="400px">
    <template v-slot:activator="{ on }">
      <v-btn v-on="on" text color="error" v-t="'core.delete'" :loading="loading" />
    </template>
    <v-card>
      <v-form v-model="valid">
        <v-card-title>
          <span class="headline" v-t="'shift.team.delete'" />
        </v-card-title>
        <v-card-text>
          <p class="subtitle-1" v-t="'shift.team.deleteDescription'" />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn text v-t="'core.cancel'" @click.stop="dialog = false" />
          <v-btn
            text
            type="submit"
            color="error"
            v-t="'core.delete'"
            :loading="loading"
            :disabled="!valid"
            @click.prevent="deleteTeam"
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
import TeamModule from '../../store/teams'
import { Project, Team } from '../../models'

@Component
export default class DeleteTeamDialog extends Vue {
  private teamModule = getModule(TeamModule, this.$store)

  @Prop(Team)
  private readonly team?: Team

  private valid = false
  private dialog = false
  private loading = false

  private async deleteTeam() {
    if (this.team) {
      this.loading = true

      await this.teamModule.deleteTeam(this.team.id)

      this.loading = false
    }

    this.dialog = false
  }
}
</script>
