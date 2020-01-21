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
        <span v-if="description">
          {{ $t(description) }}
          <i v-if="!isRequired">({{ $t('core.optional') }})</i>
        </span>
        <v-text-field
          v-on="on"
          :value="getFormattedTime"
          :label="label ? $t(label) : false"
          prepend-icon="access_time"
          :rules="getRules"
          readonly
          required
          @click="opened"
        />
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
          @click="$refs.timePicker.save(time)"
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

@Component
export default class TimeControl extends Vue {
  private userModule = getModule(UserModule, this.$store)

  @Prop(Object)
  private readonly model?: { value: string }

  @Prop(String)
  private readonly label?: string

  @Prop(String)
  private readonly description?: string

  @Prop(Boolean)
  private readonly isRequired?: boolean

  @Prop(Object)
  private readonly customRules?: any[]

  private timePicker: any = false
  private time: string = ''

  private get getFormattedTime() {
    if (this.model && this.userModule.getUser) {
      return this.userModule.getUser.formatTime(this.model.value)
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
    this.time = this.model?.value || ''
  }

  private rules = [
    (v: string) => this.isRequired === false
                   || !!v
                   || i18n.t('core.fieldRequired'),
  ]

}
</script>
