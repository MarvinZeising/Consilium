<template>
  <v-dialog
    v-model="dialog"
    max-width="600px"
  >
    <template v-slot:activator="{ on }">
      <v-btn-toggle
        dense
        class="mr-4"
        @click="opened"
      >
        <v-btn
          v-on="on"
          @click="opened"
        >
          Add Shift
        </v-btn>
      </v-btn-toggle>
    </template>
    <v-card>
      <v-form
        v-model="valid"
        ref="form"
      >
        <v-toolbar
          flat
          color="accent"
        >
          <v-toolbar-title v-t="'shift.create'" />
        </v-toolbar>
        <v-card-text>
          <i
            class="subtitle-1"
            v-t="'shift.createDescription'"
          />
        </v-card-text>
        <v-card-text class="pa-2">
          <v-layout wrap>

            <DateControl
              :model="dateModel"
              label="shift.date"
            />

            <TimeControl
              :model="timeModel"
              label="shift.time"
            />

          </v-layout>
        </v-card-text>

        <v-divider />

        <v-card-actions>
          <v-btn
            text
            v-t="'core.cancel'"
            @click.stop="dialog = false"
          />
          <v-spacer />
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
import moment from 'moment'
import { Vue, Component, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import ProjectModule from '../../store/projects'
import ShiftModule from '../../store/shifts'
import NameControl from '../controls/NameControl.vue'
import DateControl from '../controls/DateControl.vue'
import TimeControl from '../controls/TimeControl.vue'
import TextControl from '../controls/TextControl.vue'
import TextareaControl from '../controls/TextareaControl.vue'
import { Category, Eligibility, Task, Shift } from '../../models'

@Component({
  components: {
    NameControl,
    DateControl,
    TimeControl,
    TextControl,
    TextareaControl,
  },
})
export default class CreateTaskDialog extends Vue {
  private projectModule = getModule(ProjectModule, this.$store)
  private shiftModule = getModule(ShiftModule, this.$store)

  @Prop(String)
  private readonly date?: string

  private dialog: any = false
  private valid: any = null
  private loading: boolean = false

  private shift = Shift.create({
    categoryId: 'bd711f3f-f6f8-4e94-81ec-c724fa1c5d94'
  })

  private dateModel = { value: '' }
  private timeModel = { value: '10:00' }
  private durationModel = { value: '02:00' }

  private opened() {
    this.dateModel = { value: this.date || '' }
  }

  private async save() {
    if (this.valid) {
      this.loading = true

      this.shift.start = this.dateModel.value + 'T' + this.timeModel.value
      this.shift.end = moment(this.shift.start).add(2, 'h').format('YYYY-MM-DD[T]HH:mm')

      await this.shiftModule.createShift(this.shift)

      this.loading = false
      this.dialog = false
    }
  }

}
</script>
