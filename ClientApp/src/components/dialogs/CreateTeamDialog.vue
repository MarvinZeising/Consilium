<template>
  <v-dialog v-model="dialog" max-width="1000px">
    <template v-slot:activator="{ on }">
      <v-btn v-on="on" text v-t="'shift.team.create'" />
    </template>
    <v-card>
      <v-form v-model="valid" ref="form">
        <v-toolbar flat color="accent">
          <v-toolbar-title v-t="'shift.team.create'" />
          <v-spacer />
          <v-btn icon class="mr-0" @click="showHelp = !showHelp">
            <v-icon v-if="showHelp">help_outline</v-icon>
            <v-icon v-else>help</v-icon>
          </v-btn>
        </v-toolbar>

        <v-card-text v-if="showHelp">
          <i class="subtitle-1" v-t="'shift.team.createDescription'" />
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
              :maxLength="1000"
              :showDescription="showHelp"
            />

            <TextControl
              :model="helpLinkModel"
              label="shift.team.helpLink"
              description="shift.team.helpLinkDescription"
              :maxLength="1000"
              :customRules="linkRules"
              :showDescription="showHelp"
            />
          </v-layout>
        </v-card-text>

        <v-divider />

        <v-card-actions>
          <v-spacer />
          <v-btn text v-t="'core.cancel'" @click.stop="dialog = false" />
          <v-btn
            text
            type="submit"
            color="primary"
            v-t="'core.save'"
            :loading="loading"
            :disabled="!valid"
            @click.prevent="save"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import ProjectModule from '../../store/projects'
import TeamModule from '../../store/teams'
import NameControl from '../controls/NameControl.vue'
import TextControl from '../controls/TextControl.vue'
import TextareaControl from '../controls/TextareaControl.vue'
import { Category, Eligibility, Team } from '../../models'

@Component({
  components: {
    NameControl,
    TextControl,
    TextareaControl,
  },
})
export default class CreateTeamDialog extends Vue {
  private projectModule = getModule(ProjectModule, this.$store)
  private teamModule = getModule(TeamModule, this.$store)

  private dialog = false
  private valid: any = null
  private loading = false
  private showHelp = false

  private team = Team.create({})
  private nameModel = { value: '' }
  private descriptionModel = { value: '' }
  private helpLinkModel = { value: '' }
  private linkRules = [
    (v: string) => !v || /^https:\/\/.+\..{2,}$/.test(v) || i18n.t('shift.team.helpLinkFormat'),
  ]

  private async save() {
    if (this.valid) {
      this.loading = true

      this.team.name = this.nameModel.value
      this.team.description = this.descriptionModel.value
      this.team.helpLink = this.helpLinkModel.value

      await this.teamModule.createTeam(this.team)

      this.loading = false
      this.dialog = false
    }
  }
}
</script>
