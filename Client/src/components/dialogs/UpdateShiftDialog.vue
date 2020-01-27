<template>
  <v-dialog
    v-model="dialog"
    max-width="600px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        icon
        :title="$t('shift.update')"
        @click="opened"
      >
        <v-icon>edit</v-icon>
      </v-btn>
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
          <v-toolbar-title v-t="'shift.update'" />
          <v-spacer />
          <v-btn
            icon
            class="mr-0"
            @click="showHelpText = !showHelpText"
          >
            <v-icon>help</v-icon>
          </v-btn>
        </v-toolbar>
        <v-card-text v-if="showHelpText">
          <i
            class="subtitle-1"
            v-t="'shift.updateDescription'"
          />
        </v-card-text>
        <v-card-text class="pa-2">
          <v-layout wrap>

            <!--//TODO: only display category I have write permissions for -->
            <CategoryControl
              :model="categoryModel"
              required
            />

            <DateControl
              :model="dateModel"
              label="shift.date"
              required
            />

            <TimeControl
              :model="timeModel"
              label="shift.time"
              required
            />

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
          <v-btn
            text
            v-t="'core.cancel'"
            @click.stop="dialog = false"
          />
          <DeleteShiftDialog :shift="shift" />
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
import DateControl from '../controls/DateControl.vue'
import TimeControl from '../controls/TimeControl.vue'
import TextControl from '../controls/TextControl.vue'
import CategoryControl from '../controls/CategoryControl.vue'
import DeleteShiftDialog from '../dialogs/DeleteShiftDialog.vue'
import { Category, Shift } from '../../models'

@Component({
  components: {
    DateControl,
    TimeControl,
    TextControl,
    CategoryControl,
    DeleteShiftDialog,
  },
})
export default class UpdateShiftDialog extends Vue {
  private projectModule = getModule(ProjectModule, this.$store)
  private shiftModule = getModule(ShiftModule, this.$store)

  @Prop(Shift)
  private readonly shift?: Shift

  private dialog: any = false
  private valid: any = null
  private showHelpText = false
  private loading = false

  private categoryModel: { value?: Category } = {}
  private dateModel = { value: 0 }
  private timeModel = { value: 1000 }
  private durationModel = { value: 200 }

  private opened() {
    if (this.shift) {
      this.categoryModel = { value: this.shift.category }
      this.dateModel = { value: this.shift.date }
      this.timeModel = { value: this.shift.time }
      this.durationModel = { value: this.shift.duration }
    }
  }

  private async save() {
    if (this.valid && this.shift) {
      this.loading = true

      const oldCategoryId = this.shift.categoryId !== this.categoryModel?.value?.id
        ? this.shift.categoryId + ''
        : undefined

      this.shift.categoryId = this.categoryModel?.value?.id || ''
      this.shift.date = this.dateModel.value
      this.shift.time = this.timeModel.value
      this.shift.duration = this.durationModel.value

      const shift: any = this.shift
      shift.oldCategoryId = oldCategoryId

      await this.shiftModule.updateShift(shift)

      this.loading = false
      this.dialog = false
    }
  }

}
</script>
