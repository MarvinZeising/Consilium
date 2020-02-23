<template>
  <v-dialog v-model="dialog" max-width="1000px">
    <template v-slot:activator="{ on }">
      <v-btn v-on="on" text v-t="'core.edit'" @click="opened" />
    </template>
    <v-card>
      <v-form v-model="valid" ref="form">
        <v-toolbar flat color="accent">
          <v-toolbar-title v-t="'shift.team.update'" />
          <v-spacer />
          <v-btn icon class="mr-0" @click="showHelp = !showHelp">
            <v-icon v-if="showHelp">help_outline</v-icon>
            <v-icon v-else>help</v-icon>
          </v-btn>
        </v-toolbar>

        <v-card-text v-if="showHelp">
          <i class="subtitle-1" v-t="'shift.team.updateDescription'" />
        </v-card-text>

        <v-card-text class="pa-2">
          <v-layout wrap>
            <NameControl
              :model="nameModel"
              description="shift.team.nameDescription"
              :showDescription="showHelp"
            />

            <TextareaControl
              :model="descriptionModel"
              label="core.description"
              description="shift.team.descriptionDescription"
              :showDescription="showHelp"
              :maxLength="1000"
            />

            <TextControl
              :model="helpLinkModel"
              label="shift.team.helpLink"
              description="shift.team.helpLinkDescription"
              :showDescription="showHelp"
              :maxLength="1000"
              :customRules="linkRules"
            />
          </v-layout>
        </v-card-text>

        <v-divider />

        <v-card-actions>
          <v-btn text v-t="'core.cancel'" @click.stop="dialog = false" />
          <v-spacer />
          <DeleteTeamDialog :team="team" />
          <v-btn
            text
            type="submit"
            color="primary"
            v-t="'core.save'"
            :loading="loading"
            :disabled="!valid"
            @click.stop="save"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import TeamModule from '../../store/teams'
import { Role, Team } from '../../models'
import DeleteTeamDialog from './DeleteTeamDialog.vue'
import NameControl from '../controls/NameControl.vue'
import TextControl from '../controls/TextControl.vue'
import TextareaControl from '../controls/TextareaControl.vue'

@Component({
  components: {
    DeleteTeamDialog,
    NameControl,
    TextControl,
    TextareaControl,
  },
})
export default class UpdateTeamDialog extends Vue {
  private teamModule = getModule(TeamModule, this.$store)

  @Prop(Team)
  private readonly team?: Team

  private dialog = false
  private valid: any = null
  private loading = false
  private showHelp = false

  private nameModel = { value: '' }
  private descriptionModel = { value: '' }
  private helpLinkModel = { value: '' }
  private linkRules = [(v: string) => !v || /^https:\/\/.+\..{2,}$/.test(v) || i18n.t('shift.team.helpLinkFormat')]

  private opened() {
    this.nameModel = { value: this.team?.name || '' }
    this.descriptionModel = { value: this.team?.description || '' }
    this.helpLinkModel = { value: this.team?.helpLink || '' }
  }

  private async save() {
    if (this.valid && this.team) {
      this.loading = true

      this.team.name = this.nameModel.value
      this.team.description = this.descriptionModel.value
      this.team.helpLink = this.helpLinkModel.value

      await this.teamModule.updateTeam(this.team)

      this.loading = false
      this.dialog = false
    }
  }
}
</script>
