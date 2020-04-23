<template>
  <v-dialog v-model="dialog" max-width="600px">
    <template v-slot:activator="{ on }">
      <v-btn v-on="on" color="accent" dark fixed right bottom fab @click="opened">
        <v-icon>add</v-icon>
      </v-btn>
    </template>
    <v-card>
      <v-form v-model="valid" ref="form">
        <v-toolbar flat color="accent">
          <v-toolbar-title v-t="'shift.create'" />
          <v-spacer />
          <v-btn icon class="mr-0" @click="showHelp = !showHelp">
            <v-icon>help</v-icon>
          </v-btn>
        </v-toolbar>
        <v-card-text v-if="showHelp">
          <i class="subtitle-1" v-t="'shift.createDescription'" />
        </v-card-text>
        <v-card-text class="pa-2">
          <v-layout wrap>
            <!--//TODO: only display category I have write permissions for -->
            <CategoryControl :model="categoryModel" required />

            <DateControl :model="dateModel" label="shift.date" required />

            <TimeControl :model="timeModel" label="shift.time" required />

            <TimeControl
              :model="durationModel"
              label="shift.duration"
              :format="`H:mm [${$t('shift.hours')}]`"
              required
            />
          </v-layout>
        </v-card-text>

        <v-divider />

        <v-card-actions>
          <v-btn text v-t="'core.cancel'" @click.stop="dialog = false" />
          <v-spacer />
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
import moment from 'moment'
import { Vue, Component, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import ProjectModule from '../../store/projects'
import ShiftModule from '../../store/shifts'
import DateControl from '../controls/DateControl.vue'
import TimeControl from '../controls/TimeControl.vue'
import TextControl from '../controls/TextControl.vue'
import CategoryControl from '../controls/CategoryControl.vue'
import { Category, Eligibility, Team, Shift } from '../../models'

@Component({
  components: {
    DateControl,
    TimeControl,
    TextControl,
    CategoryControl,
  },
})
export default class CreateShiftDialog extends Vue {
  private projectModule = getModule(ProjectModule, this.$store)
  private shiftModule = getModule(ShiftModule, this.$store)

  @Prop(String)
  private readonly date?: string

  @Prop(Object)
  private readonly categoryModel?: { value?: Category }

  private dialog = false
  private valid: any = null
  private showHelp = false
  private loading = false

  private shift = Shift.create({})

  private dateModel = { value: 0 }
  private timeModel = { value: 1000 }
  private durationModel = { value: 200 }

  private opened() {
    this.dateModel = {
      value: parseInt(moment(this.date).format('YYYYMMDD'), 10),
    }
  }

  private async save() {
    if (this.valid) {
      this.loading = true

      this.shift.categoryId = this.categoryModel?.value?.id || ''
      this.shift.date = this.dateModel.value
      this.shift.time = this.timeModel.value
      this.shift.duration = this.durationModel.value

      await this.shiftModule.createShift(this.shift)

      this.loading = false
      this.dialog = false
    }
  }
}
</script>
