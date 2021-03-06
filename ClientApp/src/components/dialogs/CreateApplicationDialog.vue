<template>
  <v-dialog v-model="dialog" max-width="500px">
    <template v-slot:activator="{ on }">
      <v-btn v-on="on" v-if="shift.isPlanned" text v-t="'shift.application.apply'" @click="opened" />
      <v-btn
        v-on="on"
        v-else-if="shift.isScheduled"
        text
        v-t="'shift.application.applyAsBackup'"
        @click="opened"
      />
    </template>
    <v-card>
      <v-form ref="form" v-model="valid">
        <v-toolbar flat color="accent">
          <v-toolbar-title v-if="shift.isPlanned" v-t="'shift.application.create'" />
          <v-toolbar-title v-else-if="shift.isScheduled" v-t="'shift.application.createBackup'" />

          <v-spacer />

          <v-btn icon class="mr-0" @click="showHelp = !showHelp">
            <v-icon>help</v-icon>
          </v-btn>
        </v-toolbar>

        <v-card-text v-if="showHelp">
          <i v-if="shift.isPlanned" class="subtitle-1" v-t="'shift.application.createDescription'" />
          <i
            v-else-if="shift.isScheduled"
            class="subtitle-1"
            v-t="'shift.application.createBackupDescription'"
          />
        </v-card-text>
        <v-card-text class="pa-2">
          <SwitchControl
            v-if="shift.isPlanned"
            :model="availableAfterModel"
            :label="$t('shift.application.availableAfter')"
            :columns="1"
          />

          <div class="special-notes">
            <TextareaControl
              :model="notesModel"
              :label="$t('shift.application.notes')"
              :description="$t('shift.application.notesDescription')"
              :showDescription="true"
              :columns="1"
              :maxLength="200"
            />
          </div>
        </v-card-text>

        <v-divider />

        <v-card-actions>
          <v-btn text v-t="'core.cancel'" @click.stop="dialog = false" />
          <v-spacer />
          <v-btn
            text
            type="submit"
            color="primary"
            v-t="'shift.application.apply'"
            :loading="loading"
            :disabled="!valid"
            @click.prevent="save"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<style lang="scss" scoped>
.special-notes {
  margin-top: 15px;
}
</style>

<script lang="ts">
import moment from 'moment'
import { Vue, Component, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import PersonModule from '../../store/persons'
import ApplicationModule from '../../store/applications'
import { Application, Category, Shift } from '../../models'
import SwitchControl from '../controls/SwitchControl.vue'
import TextareaControl from '../controls/TextareaControl.vue'

@Component({
  components: { SwitchControl, TextareaControl },
})
export default class CreateApplicationDialog extends Vue {
  private personModule = getModule(PersonModule, this.$store)
  private applicationModule = getModule(ApplicationModule, this.$store)

  @Prop(Shift)
  private readonly shift?: Shift

  private dialog = false
  private valid: any = null
  private showHelp = false
  private loading = false

  private availableAfterModel = { value: false }
  private notesModel = { value: '' }

  private application = Application.create({})

  private opened() {
    if (this.shift) {
      this.application.shiftId = this.shift.id
      this.application.personId = this.personModule.getActivePersonId || ''
    }
  }

  private async save() {
    if (this.valid) {
      this.loading = true

      this.application.availableAfter = this.availableAfterModel.value
      this.application.notes = this.notesModel.value

      await this.applicationModule.createApplication(this.application)

      this.loading = false
      this.dialog = false
    }
  }
}
</script>
