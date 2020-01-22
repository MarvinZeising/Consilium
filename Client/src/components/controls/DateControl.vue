<template>
  <v-flex
    xs12 sm6
    class="pa-2"
  >
    <v-dialog
      ref="datePicker"
      v-model="datePicker"
      :return-value.sync="model.value"
      width="290px"
    >
      <template v-slot:activator="{ on }">
        <div
          v-on="on"
          @click="opened"
        >
          <span v-if="description">
            {{ $t(description) }}
            <i v-if="!required">({{ $t('core.optional') }})</i>
          </span>
          <v-text-field
            :value="getFormattedDate"
            :label="label ? $t(label) : false"
            :rules="getRules"
            readonly
            filled
            required
          />
        </div>
      </template>
      <v-date-picker
        v-if="datePicker"
        v-model="date"
        :first-day-of-week="1"
        scrollable
      >
        <v-btn
          text
          v-t="'core.cancel'"
          @click="datePicker = false"
        />
        <v-spacer />
        <v-btn
          text
          color="primary"
          v-t="'core.ok'"
          @click="updateDate"
        />
      </v-date-picker>
    </v-dialog>

  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import UserModule from '../../store/users'
import moment from 'moment'

@Component
export default class DateControl extends Vue {
  private userModule = getModule(UserModule, this.$store)

  @Prop(Object)
  private readonly model?: { value: number }

  @Prop(String)
  private readonly label?: string

  @Prop(String)
  private readonly description?: string

  @Prop(Object)
  private readonly customRules?: any[]

  @Prop(Boolean)
  private required: boolean = false

  private datePicker: any = false
  private date: string = ''
  private rules = [
    (v: string) => !this.required
                   || !!v
                   || i18n.t('core.fieldRequired'),
  ]

  private get getFormattedDate() {
    if (this.model && this.userModule.getUser) {
      return this.userModule.getUser.formatDate(moment(this.model.value, 'YYYYMMDD').format())
    }

    return this.model?.value || ''
  }

  private get getRules() {
    if (this.customRules) {
      return this.rules.concat(this.customRules)
    }
    return this.rules
  }

  private opened() {
    this.date = moment(this.model?.value, 'YYYYMMDD').format('YYYY-MM-DD')
  }

  private updateDate() {
    if (this.model) {
      const picker: any = this.$refs.datePicker
      picker.save(parseInt(moment(this.date, 'YYYY-MM-DD').format('YYYYMMDD'), 10))
      this.datePicker = false
    }
  }

}
</script>
