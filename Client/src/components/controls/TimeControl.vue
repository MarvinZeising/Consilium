<template>
  <v-flex
    xs12 sm6
    class="pa-2"
  >
    <v-dialog
      ref="timePicker"
      v-model="timePicker"
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
            :value="getFormattedTime"
            :label="label ? $t(label) : false"
            readonly
            filled
            required
          />
        </div>
      </template>
      <v-time-picker
        v-if="timePicker"
        v-model="time"
        format="24hr"
      >
        <v-btn
          text
          v-t="'core.cancel'"
          @click="timePicker = false"
        />
        <v-spacer />
        <v-btn
          text
          color="primary"
          v-t="'core.ok'"
          @click="updateTime"
        />
      </v-time-picker>
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
export default class TimeControl extends Vue {
  private userModule = getModule(UserModule, this.$store)

  @Prop(Object)
  private readonly model?: { value: number }

  @Prop(String)
  private readonly label?: string

  @Prop(String)
  private readonly description?: string

  @Prop(String)
  private readonly format?: string

  @Prop(Boolean)
  private required = false

  private timePicker: any = false
  private time: string = ''
  private rules = [
    (v: string) => !this.required
                   || !!v
                   || i18n.t('core.fieldRequired'),
  ]

  private get getFormattedTime() {
    if (this.format) {
      return moment(this.model?.value, 'Hmm').format(this.format)
    }
    if (this.userModule.getUser) {
      return this.userModule.getUser.formatTime(moment(this.model?.value, 'Hmm').format('H:mm'))
    }
    return this.model?.value || ''
  }

  private opened() {
    this.time = moment(this.model?.value, 'Hmm').format('H:mm')
  }

  private updateTime() {
    if (this.model) {
      const picker: any = this.$refs.timePicker
      picker.save(parseInt(moment(this.time, 'H:mm').format('Hmm'), 10))
      this.timePicker = false
    }
  }

}
</script>
